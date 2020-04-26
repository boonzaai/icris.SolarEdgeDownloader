using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace icris.SolarEdgeDownloader
{
    class ApiConnector
    {
        public async Task<List<Measurement>> GetData(DateTime startDate, DateTime endDate, string apiKey)
        {
            var start = startDate.ToString("yyyy-MM-dd");
            var end = endDate.ToString("yyyy-MM-dd");
            
            var dataPower = await new HttpClient().GetAsync($"https://monitoringapi.solaredge.com/site/77054/power.json?startTime={start}&endTime={end}&api_key={apiKey}");
            var dataEnergy = await new HttpClient().GetAsync("https://monitoringapi.solaredge.com/site/77054/energy.json?timeUnit=QUARTER_OF_AN_HOUR&endDate=" + end + "&startDate=" + start + "&api_key=");
            var contentPower = await dataPower.Content.ReadAsStringAsync();
            var valuesPower = JObject.Parse(contentPower);
            var valuesEnergy = JObject.Parse(await dataEnergy.Content.ReadAsStringAsync());
            //var modname = vals.body.devices[0].module_name;
            List<Measurement> measurements = new List<Measurement>();
            for (var i = 0; i < ((JArray)valuesPower["power"]["values"]).Count; i++)
            {
                Measurement measurement = new Measurement();
                var power = ((JArray)valuesPower["power"]["values"])[i];
                var energy = ((JArray)valuesEnergy["energy"]["values"])[i];                
                var date = DateTime.ParseExact(power.Value<string>("date"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);                
                measurement.Timestamp = date;
                measurement.Power = power.Value<double?>("value");
                measurement.Energy = energy.Value<double?>("value");
                measurements.Add(measurement);
            }

            return measurements;


        }
    }
}
