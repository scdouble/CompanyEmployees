using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CompanyEmployees.ModelBinders
{
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // We are creating a model binder for the IEnumerable type.
            // Therefore, we have to check if our parameter is the same type.
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            // extract the value (a comma-separated string of GUIDs) with the ValueProvider.GetValue() expression.
            var providedValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();
            // Because it is type string, we just check whether it is null or empty.
            if (string.IsNullOrEmpty(providedValue))
            {
                //  If it is, we return null as a result because we have a null check in our action in the controller.
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            // In the genericType variable, with the reflection help,
            // we store the type the IEnumerable consists of. In our case, it is GUID. 
            var genericType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];

            // with the converter variable, we create a converter to a GUID type.
            var converter = TypeDescriptor.GetConverter(genericType);

            var objectArray = providedValue.Split(new[] {","},
                    StringSplitOptions.RemoveEmptyEntries)
                .Select(x => converter.ConvertFromString(x.Trim()))
                .ToArray();

            // we create an array of type object (objectArray)
            // that consist of all the GUID values we sent to the API and then create an array of GUID types (guidArray),
            // copy all the values from the objectArray to the guidArray
            var guidArray = Array.CreateInstance(genericType, objectArray.Length);
            objectArray.CopyTo(guidArray, 0);
            bindingContext.Model = guidArray;
            
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
            ;
        }
    }
}