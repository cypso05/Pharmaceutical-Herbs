using MongoExample.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc;

namespace MongoExample.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<PharmaGuideCollections> _PharmaGuideCollection;

        // Constructor to initialize MongoDB client and collection
        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            // Initialize MongoDB client and collection from settings
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _PharmaGuideCollection = database.GetCollection<PharmaGuideCollections>(mongoDBSettings.Value.CollectionName);
        }

        // Create a new document in the collection
        public async Task CreateAsync(PharmaGuideCollections pharmaguidecollections)
        {
            await _PharmaGuideCollection.InsertOneAsync(pharmaguidecollections);
        }

        // Get all documents from the collection
        public async Task<List<PharmaGuideCollections>> GetAsync()
        {
            return await _PharmaGuideCollection.Find(new BsonDocument()).ToListAsync();
        }

        // Update an existing document in the collection by matching its ID
        public async Task UpdatePharmaGuideAsync(string id, HerbalDrugUpdate update)
        {
            var filter = Builders<PharmaGuideCollections>.Filter.Eq(d => d._id, new ObjectId(id));

            var updateDefinition = Builders<PharmaGuideCollections>.Update
                .Set(d => d.Status, update.Status)
                .Set(d => d.Herb_Latin_Name, update.Herb_Latin_Name)
                .Set(d => d.Botanical_Name, update.Botanical_Name)
                .Set(d => d.English_common_name, update.English_Common_Name)
                .Set(d => d.Combination, update.Combination)
                .Set(d => d.Use, update.Use)
                .Set(d => d.Outcome, update.Outcome)
                .Set(d => d.Date_added_to_the_inventory, update.Date_Added_To_The_Inventory)
                .Set(d => d.Date_added_to_the_priority_list, update.Date_Added_To_The_Priority_List)
                .Set(d => d.First_published, update.First_Published)
                .Set(d => d.Revision_date, update.Revision_Date)
                .Set(d => d.More_Information, update.More_Information);

            // Perform the update operation
            await _PharmaGuideCollection.UpdateOneAsync(filter, updateDefinition);
        }

        // Delete a document from the collection by matching its ID
        public async Task<DeleteResult> DeleteFromPharmaGuideAsync(FilterDefinition<PharmaGuideCollections> filter)
        {
            return await _PharmaGuideCollection.DeleteOneAsync(filter);
        }

        // Optional: Get a document by its ID for other potential use cases
        public async Task<PharmaGuideCollections> GetByIdAsync(string id)
        {
            var filter = Builders<PharmaGuideCollections>.Filter.Eq(d => d._id, new ObjectId(id));
            return await _PharmaGuideCollection.Find(filter).FirstOrDefaultAsync();
        }

        // Search for documents based on Herb_Latin_Name, Botanical_Name, and English_common_name
        public async Task<List<PharmaGuideCollections>> GetPharmaGuideBySearchAsync(
            string? herbLatinName = null,
            string? botanicalName = null,
            string? englishCommonName = null,
            string? use = null)
        {
            // Build a filter that matches any of the provided fields
            var filterBuilder = Builders<PharmaGuideCollections>.Filter;
            FilterDefinition<PharmaGuideCollections> filter = FilterDefinition<PharmaGuideCollections>.Empty; // Start with an empty filter

            // Add filters dynamically based on user input
            if (!string.IsNullOrEmpty(herbLatinName))
            {
                // Case-insensitive, trim spaces, and allow partial matches
                herbLatinName = herbLatinName.Trim().ToLower();
                filter = filterBuilder.And(filter, filterBuilder.Regex(d => d.Herb_Latin_Name, new BsonRegularExpression($"^{herbLatinName}", "i")));
            }

            if (!string.IsNullOrEmpty(botanicalName))
            {
                // Case-insensitive, trim spaces, and allow partial matches
                botanicalName = botanicalName.Trim().ToLower();
                filter = filterBuilder.And(filter, filterBuilder.Regex(d => d.Botanical_Name, new BsonRegularExpression($"^{botanicalName}", "i")));
            }

            if (!string.IsNullOrEmpty(englishCommonName))
            {
                // Case-insensitive, trim spaces, and allow partial matches
                englishCommonName = englishCommonName.Trim().ToLower();
                filter = filterBuilder.And(filter, filterBuilder.Regex(d => d.English_common_name, new BsonRegularExpression($"^{englishCommonName}", "i")));
            }

            if (!string.IsNullOrEmpty(use))
            {
                // Case-insensitive regex for 'use' with partial match
                use = use.Trim().ToLower();
                filter = filterBuilder.And(filter, filterBuilder.Regex(d => d.Use, new BsonRegularExpression(use, "i")));
            }

            // Execute the query and return the matching documents
            var result = await _PharmaGuideCollection.Find(filter).ToListAsync();
            return result; // Return the list of matching records
        }
    }
}
