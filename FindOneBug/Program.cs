using System;
using System.IO;
using LiteDB;

namespace FindOneBug
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var database = new LiteDatabase(new MemoryStream()))
            {
                var doc1 = new BsonDocument {["name"] = "john doe"};

                // TODO Comment lines 16, 17, and 30 to illiminate the transaction, and observe the correct behavior
                using (database.BeginTrans())
                {
                    // Insert a person into the db, and confirm that the person exists
                    database.GetCollection("people").Insert(doc1);
                    var peopleCount = database.GetCollection("people").Count();
                    Console.WriteLine($"Number of people in db: {peopleCount}. Expected 1");

                    // Query the db for the same person. Confirm that the person is returned.
                    var person = database.GetCollection("people").FindOne(Query.EQ("name", "john doe"));
                    Console.WriteLine($"Person is null? {person == null}");

                    // Again, check the count of people. We expect this to be 1.
                    peopleCount = database.GetCollection("people").Count();
                    Console.WriteLine($"Number of people in db: {peopleCount}. Expected 1");
                }
            }
        }
    }
}