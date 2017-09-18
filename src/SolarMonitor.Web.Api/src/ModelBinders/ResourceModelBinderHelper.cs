using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SolarMonitor.Data.Resources;
using SolarMonitor.Data.Repositories;
using CommonTypes = SolarMonitor.Data.CommonTypes;

namespace SolarMonitorApi.ModelBinders
{
    public class ResourceModelBinderHelper<EntityT, TypeEnumT>
        where EntityT : Resource
        where TypeEnumT : struct, IConvertible
    {
        public EntityT GetEntity(Stream requestBody)
        {
            string jsonData = string.Empty;
            using (var sr = new StreamReader(requestBody))
            {
                jsonData = sr.ReadToEnd();
            }

            if (string.IsNullOrEmpty(jsonData))
            {
                return default(EntityT);
            }

            string entityTypeAsString = (string)JObject.Parse(jsonData)["type"];

            // try to parse the string to ensure the type was specified correctly
            var success = Enum.TryParse(typeof(TypeEnumT), entityTypeAsString, false, out var entityType);
            if (!success)
            {
                return default(EntityT);
            }

            var typeName = $"{typeof(EntityT).Namespace}.{entityTypeAsString}, {typeof(EntityT).GetTypeInfo().Assembly}";
            var devType = Type.GetType(typeName);
            var entity = (EntityT)JsonConvert.DeserializeObject(jsonData, devType);

            return entity;
        }
    }
}