using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements]
public class PharmaGuideCollections
{
    [BsonId]
    public ObjectId _id { get; set; }

    public string? Status { get; set; } = "Unknown";

    public string Herb_Latin_Name { get; set; } = string.Empty;
    public string Botanical_Name { get; set; } = string.Empty;
    public string English_common_name { get; set; } = string.Empty;
    public string Combination { get; set; } = string.Empty;
    public string Use { get; set; } = string.Empty;
    public string Outcome { get; set; } = string.Empty;

    [BsonElement("Date_added_to_the_inventory")]
    public DateTime Date_added_to_the_inventory { get; set; } = DateTime.MinValue;

    [BsonElement("Date_added_to_the_priority_list")]
    public DateTime Date_added_to_the_priority_list { get; set; } = DateTime.MinValue;

    [BsonElement("First_published")]
    public DateTime First_published { get; set; } = DateTime.MinValue;

    [BsonElement("Revision_date")]
    public DateTime Revision_date { get; set; } = DateTime.MinValue;

    public string More_Information { get; set; } = string.Empty;
}

public class HerbalDrugUpdate
{
    public string DrugId { get; set; } = "Unknown";
    public string Status { get; set; } = "Unknown";
    public string Herb_Latin_Name { get; set; } = string.Empty;
    public string Botanical_Name { get; set; } = string.Empty;
    public string English_Common_Name { get; set; } = string.Empty;
    public string Combination { get; set; } = string.Empty;
    public string Use { get; set; } = string.Empty;
    public string Outcome { get; set; } = string.Empty;
    public DateTime Date_Added_To_The_Inventory { get; set; }
    public DateTime Date_Added_To_The_Priority_List { get; set; }
    public DateTime First_Published { get; set; }
    public DateTime Revision_Date { get; set; }
    public string More_Information { get; set; } = string.Empty;
}
