using System;
using System.Reflection;
using BookStore.BackOffice.WebApi.BookStoreProperties;

namespace BookStore.BackOffice.WebApi.HelperClasses
{
    public class Helpers : IHelpers
    {
        private IPropertiesOfDataToInsert propertiesToInsert;
        private IBookDataProperty bookDataProperty;

        public Helpers(IPropertiesOfDataToInsert propertiesToInsert, IBookDataProperty bookDataProperty)
        {
            this.propertiesToInsert = propertiesToInsert;
            this.bookDataProperty = bookDataProperty;
        }

        public void ToDictionary(string[] headerTitles)
        {

            if (propertiesToInsert == null)
            {
                throw new Exception($"Object {propertiesToInsert} is null");
            }

            foreach (var name in headerTitles)
            {
                var keyName = ProduceKey(name);

                if (name.ToLower() == "author")
                {

                    propertiesToInsert.HeaderTitle.Add(keyName, "Author");

                    continue;
                }

                var value = bookDataProperty.BookStore.GetType().GetProperty(
                            name.ToLower(),
                                BindingFlags.IgnoreCase |
                                BindingFlags.Public |
                                BindingFlags.Instance)
                                    .Name;

                propertiesToInsert.HeaderTitle.Add(keyName, value);
            }
        }

        private string ProduceKey(string name)
        {
            // could be somewhere as constant
            var expectedRowTitles = new string[]
            {
                "Title",
                "Author",
                "Price",
                "Best Seller",
                "Availability",
                "Description",
                "Year Published"
            };

            switch (name.ToLower())
            {
                case "title":
                    return expectedRowTitles[0];
                case "author":
                    return expectedRowTitles[1];
                case "price":
                    return expectedRowTitles[2];
                case "isbestseller":
                    return expectedRowTitles[3];
                case "availablestock":
                    return expectedRowTitles[4];
                case "shortdescription":
                    return expectedRowTitles[5];
                case "publicationyear":
                    return expectedRowTitles[6];

                default:
                    throw new Exception($"Title {name} did not match any of the default values.");
            }
        }
    }
}
