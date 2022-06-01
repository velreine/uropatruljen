using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace CommonData.Model.Action
{
    /**
     * This class looks much fancier than it is, all it does is control that we have two maps.
     * The original map that maps Action Types to identifiers (which are just simple integers).
     * And a reverse map that maps identifiers (again, integers) to Action Types.
     * 
     * This means when serializing actions we can put the correct identifier on the action.
     * And when de-serializing we can construct the correct action given the identifier.
     */
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "RedundantDefaultMemberInitializer")]
    public static class ActionMap
    {

        private static bool _mapInitialized = false;
        private static int _currentIdentifier = -1;
        
        private static readonly Dictionary<Type, int> _actionTypeToActionIdentifier = new Dictionary<Type, int>();

        /**
         * The immutable dictionary means that the underlying private backing field cannot be modified without using
         * methods defined on this class to alter it,
         * and simply retrieving the dictionary via the public property exposes an immutable type,
         * clearly hinting to the programmer that it cannot be modified directly.
         */
        public static ImmutableDictionary<Type, int> ActionTypeToActionIdentifier
        {
            get
            {
                if (_mapInitialized == false)
                {
                    InitializeMap();
                }

                return _actionTypeToActionIdentifier.ToImmutableDictionary();
            }
        }

        public static ImmutableDictionary<int, Type> ActionIdentifierToActionType
        {
            get
            {
                if (_mapInitialized == false)
                {
                    InitializeMap();
                }

                // This inverses the _actionTypeToActionIdentifier dictionary.
                // Giving us a dictionary that maps integers to types, instead of types to integers.
                return _actionTypeToActionIdentifier.ToImmutableDictionary(
                    originalDictionary => originalDictionary.Value, originalDictionary => originalDictionary.Key);
            }
        }
        
        /**
         * Initialize the map by registering our default actions.
         */
        private static void InitializeMap()
        {
            
            _actionTypeToActionIdentifier.Add(typeof(TurnOnOffAction), _currentIdentifier++);
            _actionTypeToActionIdentifier.Add(typeof(SetColorAction), _currentIdentifier++);


            // Mark the map as initialized.
            _mapInitialized = true;
        }

        /**
         * This function enables the user to RegisterCustomActions if necessary.
         * The return value is the identifier the action was given in the ActionMap.
         * Throws an exception if a non-class or non IAction is attempted to be registered.
         */
        public static int RegisterCustomAction(Type actionType) 
        {
            // Ensure the type implements the IAction interface.
            if (!actionType.GetInterfaces().Contains(typeof(IAction)))
            {
                throw new Exception("Cannot register a custom action type which does not implement IAction.");
            }

            // Ensure the type is a class.
            if (!actionType.IsClass)
            {
                throw new Exception("The custom action trying to be registered must be a class.");
            }

            // If the Action is already registered just return the identifier from the map.
            if (_actionTypeToActionIdentifier.ContainsKey(actionType))
            {
                _actionTypeToActionIdentifier.TryGetValue(actionType, out var existingIdentifier);
                return existingIdentifier;
            }
            
            // Initialize the map if it haven't been initialized.
            if (_mapInitialized == false)
            {
                InitializeMap();
            }

            // Figure out what the new identifier will be for the action we are registering.
            var givenIdentifier = _currentIdentifier++;
            
            // Add it to the map.
            _actionTypeToActionIdentifier.Add(actionType, givenIdentifier);

            // Return the newly created identifier.
            return givenIdentifier;
        }
        
        
    }
}