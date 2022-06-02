using System.Reflection;
using CommonData.Model.Entity.Contracts;

namespace HubApi.Logic;

/// <summary>
/// This is a class that is supposed to take in an anonymous object with data, and hydrate a generic type of T
/// from the data.
///
/// Side note: this was useful if we were not gonna use entity framework and roll with our own solution.
/// </summary>
public class ScalarObjectHydrator
{

    
    /**
     * Given a type T, and an object that contains data,
     * automatically assigns data to their correct fields in the type T.
     * And returns an instance of T.
     */
    public static T? HydrateType<T>(object data) where T: IEntity, new()
    {

        var output = new T();
        
        var typeProperties = typeof(T).GetProperties(BindingFlags.Public|BindingFlags.Instance);
        
        foreach (var propInfo in typeProperties)
        {
            // If this is not a scalar type, we do not know how to hydrate the property "this responsibility",
            // should be in a separate hydrator.
            if (!IsScalarType(propInfo.GetType()))
            {
                continue;
            }
            
            // Take the property name and convert it to snake case,
            // if the "data" contains that property it must belong to this property.
            // "Id" => "id"
            var snakeCasedPropertyName = StringConverter.ToSnakeCase(propInfo.Name);
            
            // Check if data object has "id".
            var value = data.GetType()?.GetProperty(snakeCasedPropertyName)?.GetValue(data, null);

            // If we successfully retrieved a value from the data object, assign it to the output.
            propInfo.SetValue(output, value);
        }

        return output;
    }

    /**
     * TODO: Test IsScalarType virker...., HydrateType hydrater ikke objekt korrekt.
     */
    public static bool IsScalarType(Type type)
    {
        var knownScalarTypes = new[] { typeof(string), typeof(char), typeof(bool), typeof(float), typeof(int), typeof(double) };

        return knownScalarTypes.Contains(type);
    }
    
}