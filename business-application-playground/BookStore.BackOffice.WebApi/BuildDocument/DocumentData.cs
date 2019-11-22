using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using BookStore.BackOffice.WebApi.BookStorePropertiesDto;
using BookStore.BackOffice.WebApi.HelperClasses;
using BookStore.BackOffice.WebApi.Models;

namespace BookStore.BackOffice.WebApi.BuildDocument
{
    public class DocumentData
    {
        private IPropertiesOfData propertiesToInsert;
        private IHelpers helper;

        public DocumentData(IPropertiesOfData propertiesToInsert, IHelpers helper)
        {
            this.propertiesToInsert = propertiesToInsert;
            this.helper = helper;
        }

        public void ToDictionary(string[] values)
        {
            if (propertiesToInsert == null)
            {
                throw new NullReferenceException(Constants.NullReferenceErr);
            }

            MapTitles(values);
        }

        private void MapTitles(string[] values)
        {
            values = SetValueToColumnTitles(values);

            var textInfo = helper.SetCultureInfo();

            foreach (var name in values)
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException(Constants.ArgumentEmptyTitleErr);

                var keyName = textInfo.ToLower(name);

                if (keyName == "author")
                {
                    propertiesToInsert.HeaderTitle.Add(keyName, textInfo.ToTitleCase(keyName));

                    continue;
                }

                keyName = textInfo.ToTitleCase(keyName);

                var value = GetPropertyName(keyName);

                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(Constants.ArgumentEmptyTitleErr);

                propertiesToInsert.HeaderTitle.Add(keyName, value);
            }
        }

        private string GetPropertyName(string key)
        {
            var bookModelDto = new BookAndItsAuthorsModelDto();

            foreach (var property in bookModelDto.GetType().GetProperties())
            {
                var val = property.GetCustomAttributesData().Any(a => a.AttributeType.Name == "DisplayNameAttribute");

                if (val == false)
                    continue;

                var attrName = property.GetCustomAttribute<DisplayNameAttribute>().DisplayName;

                if (attrName.ToString() != key)
                    continue;

                return property.Name;
            }

            return "";
        }

        private string[] SetValueToColumnTitles(string[] values)
        {
            if (helper.ArrayIsNullOrEmpty(values))
            {
                values = Constants.PrettyTitles;
            }

            return values;
        }
    }
}
