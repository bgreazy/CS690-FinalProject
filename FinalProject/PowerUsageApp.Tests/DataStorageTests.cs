namespace PowerUsageApp.Tests;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Xunit;
using PowerUsageApp;

public class DataStorageTests
{
    private readonly string testFilePath = "testEnergyData.json";

    [Fact]
    public void SaveData_ShouldCreateFileAndStoreData()
    {
        
        var storage = new DataStorage(testFilePath);
        var testRecords = new List<EnergyData>
        {
            new EnergyData(DateTime.Now, 10.5, 2.5), 
            new EnergyData(DateTime.Now.AddDays(-1), 8.3, 2.2)
        };

        
        storage.SaveData(testRecords);

        Assert.True(File.Exists(testFilePath)); 
        string json = File.ReadAllText(testFilePath);
        var loadedRecords = JsonSerializer.Deserialize<List<EnergyData>>(json);
        Assert.NotNull(loadedRecords);
        Assert.Equal(testRecords.Count, loadedRecords.Count); 
    }

    [Fact]
    public void LoadData_ShouldReturnSavedData()
    {
        
        var storage = new DataStorage(testFilePath);

        
        List<EnergyData> loadedData = storage.LoadData();

        
        Assert.NotNull(loadedData); 
        Assert.NotEmpty(loadedData); 
    }
}