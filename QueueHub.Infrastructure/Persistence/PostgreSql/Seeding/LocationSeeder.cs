using System.Text.Json;
using Domain.Entities;
using Infrastructure.Persistence.PostgreSql.EfCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.PostgreSql.Seeding;

public class LocationSeeder(QueueHubDbContext db, ILogger<LocationSeeder> logger)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private sealed record ProvinceItem(int Id, string Name);
    private sealed record CityItem(long Id, string Name, int ProvinceId);

    public async Task SeedAsync(CancellationToken ct = default)
    {
        var provinceIds = await SeedProvincesAsync(ct);
        await SeedCitiesAsync(provinceIds, ct);
    }

    private async Task<Dictionary<int, Guid>> SeedProvincesAsync(CancellationToken ct)
    {
        var items = await ReadAsync<ProvinceItem>("provinces.json", ct);
        var existing = await db.Provinces.ToDictionaryAsync(p => p.ExternalCode, ct);

        var added = 0;
        foreach (var item in items)
        {
            var name = Normalize(item.Name);

            if (existing.TryGetValue(item.Id, out var province))
            {
                if (province.Name != name) province.Name = name;
            }
            else
            {
                province = new Province
                {
                    Id = Guid.NewGuid(),
                    ExternalCode = item.Id,
                    Name = name
                };
                db.Provinces.Add(province);
                existing[item.Id] = province;
                added++;
            }
        }

        await db.SaveChangesAsync(ct);
        logger.LogInformation("Provinces seeded. Added: {Added}", added);

        return existing.ToDictionary(x => x.Key, x => x.Value.Id);
    }

    private async Task SeedCitiesAsync(Dictionary<int, Guid> provinceIds, CancellationToken ct)
    {
        var items = await ReadAsync<CityItem>("cities.json", ct);
        var existing = await db.Cities.ToDictionaryAsync(c => c.ExternalCode, ct);

        var added = 0;
        foreach (var item in items)
        {
            if (!provinceIds.TryGetValue(item.ProvinceId, out var provinceId))
                throw new InvalidOperationException(
                    $"City '{item.Name}' ({item.Id}) refers to unknown province code {item.ProvinceId}.");

            var name = Normalize(item.Name);

            if (existing.TryGetValue(item.Id, out var city))
            {
                if (city.Name != name) city.Name = name;
                if (city.ProvinceId != provinceId) city.ProvinceId = provinceId;
            }
            else
            {
                db.Cities.Add(new City
                {
                    Id = Guid.NewGuid(),
                    ExternalCode = item.Id,
                    Name = name,
                    ProvinceId = provinceId
                });
                added++;
            }
        }

        await db.SaveChangesAsync(ct);
        logger.LogInformation("Cities seeded. Added: {Added}", added);
    }

    private static async Task<List<T>> ReadAsync<T>(string fileName, CancellationToken ct)
    {
        var assembly = typeof(LocationSeeder).Assembly;

        var resourceName = assembly.GetManifestResourceNames()
                               .SingleOrDefault(n => n.EndsWith("." + fileName, StringComparison.OrdinalIgnoreCase))
                           ?? throw new InvalidOperationException(
                               $"Embedded resource '{fileName}' not found. Check the file is in SeedData and set as EmbeddedResource.");

        await using var stream = assembly.GetManifestResourceStream(resourceName)!;

        return await JsonSerializer.DeserializeAsync<List<T>>(stream, JsonOptions, ct)
               ?? throw new InvalidOperationException($"{fileName} is empty or invalid.");
    }

    private static string Normalize(string value) =>
        value.Replace('ي', 'ی').Replace('ى', 'ی').Replace('ك', 'ک').Trim();
}