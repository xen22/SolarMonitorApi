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
using SolarMonitor.Data.Models;
using SolarMonitor.Data.Repositories;
using CommonTypes = SolarMonitor.Data.CommonTypes;

namespace SolarMonitorApi.ModelBinders
{
    public class EntityModelBinderHelper<EntityT, TypeEnumT>
        where EntityT : class, IEntity
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

            int entityTypeAsInt = Convert.ToInt32(((JValue)JObject.Parse(jsonData)["typeId"]).Value);
            var entityType = (TypeEnumT)(object)entityTypeAsInt;

            var typeName = $"{typeof(EntityT).Namespace}.{entityType}, {typeof(EntityT).GetTypeInfo().Assembly}";
            var devType = Type.GetType(typeName);
            var entity = (EntityT)JsonConvert.DeserializeObject(jsonData, devType);

            return entity;
        }
    }
}