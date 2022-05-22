﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace MQTT_Server
{
    internal static class ExtensionMethods
    {
        public static TObject DumpToConsole<TObject>(this TObject @object)
        {
            var output = "NULL";
            if (@object != null)
            {
                output = JsonSerializer.Serialize(@object, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
            }

            Console.WriteLine($"[{@object?.GetType().Name}]:\r\n{output}");
            return @object;
        }

    }
}
