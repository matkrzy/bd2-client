using System;
using System.Data.Linq;
using BD_client.Dto;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using RestSharp;

namespace BD_client.Api.Utils
{
    public static class Utils
    {
        public static T Deserialize<T>(IRestResponse response) where T : new()
        {
            try
            {
                if (response.Content.Equals(""))
                {
                    return (T) Activator.CreateInstance(typeof(T));
                }

                return JsonConvert.DeserializeObject<T>(response.Content);
            }
            catch (Exception e)
            {
                return (T) Activator.CreateInstance(typeof(T));
            }
        }

        public static T Deserialize<T>(
            IRestResponse response,
            object context,
            IDialogCoordinator dialog,
            String content) where T : new()
        {
            try
            {
                if (response.Content.Equals(""))
                {
                    dialog.ShowMessageAsync(context, "Oooppss...", content);
                    return (T) Activator.CreateInstance(typeof(T));
                }

                return JsonConvert.DeserializeObject<T>(response.Content);
            }
            catch (Exception e)
            {
                dialog.ShowMessageAsync(context, "Oooppss...", content);
                return (T) Activator.CreateInstance(typeof(T));
            }
        }
    }
}