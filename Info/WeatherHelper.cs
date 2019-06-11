using System;
using System.IO;
using System.Linq;
using System.Text;
using Ji.NetHelper;

namespace Ji.DataHelper.Weather
{
    public static class WeatherHelper
    {
        public const string WeatherUrl = "http://php.weather.sina.com.cn/xml.php?city={0}&password=DJOYnieT8234jlsK&day=0";

        public static Profiles GetWeather(string cityname)
        {
            using (var stream = HttpHelper.GetStreamResponse(string.Format(WeatherUrl, StringHelper.ConvertToGB2312(cityname))))
            {
                var weather = XmlData.ReadFromStream<Profiles>(stream);
                stream.Position = 0;
                using (var sr = new StreamReader(stream, Encoding.UTF8))
                {
                    var ws = sr.ReadToEnd();
                    if (!string.IsNullOrWhiteSpace(ws))
                    {
                        var das = ws.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault(i => i.Contains("published at"));
                        if (!string.IsNullOrWhiteSpace(das))
                        {
                            var time = das.Splits(" published at", "<", ">", "--", "!");
                            if (weather.Weather != null && time.Count == 1)
                            {
                                weather.Weather.udatetime = time[0];
                                return weather;
                            }
                        }
                    }
                }
            }
            return null;
        }
    }

    public class Profiles
    {
        public Weather Weather { get; set; }
    }

    public class Weather
    {
        public string city { get; set; }

        public string status1 { get; set; }// 多云 /status1 { get; set; }//
        public string status2 { get; set; }// 多云 /status2 { get; set; }//
        public string figure1 { get; set; }// duoyun /figure1 { get; set; }//
        public string figure2 { get; set; }// duoyun /figure2 { get; set; }//
        public string direction1 { get; set; }// 无持续风向 /direction1 { get; set; }//
        public string direction2 { get; set; }// 无持续风向 /direction2 { get; set; }//
        public string power1 { get; set; }// ≤3 /power1 { get; set; }//
        public string power2 { get; set; }// ≤3 /power2 { get; set; }//
        public string temperature1 { get; set; }// 25 /temperature1 { get; set; }//
        public string temperature2 { get; set; }// 13 /temperature2 { get; set; }//
        public string ssd { get; set; }// 6 /ssd { get; set; }//
        public string tgd1 { get; set; }// 23 /tgd1 { get; set; }//
        public string tgd2 { get; set; }// 23 /tgd2 { get; set; }//
        public string zwx { get; set; }// 1 /zwx { get; set; }//
        public string ktk { get; set; }// 4 /ktk { get; set; }//
        public string pollution { get; set; }// 3 /pollution { get; set; }//
        public string xcz { get; set; }// 4 /xcz { get; set; }//
        public string zho { get; set; }//  /zho { get; set; }//
        public string diy { get; set; }//  /diy { get; set; }//
        public string fas { get; set; }//  /fas { get; set; }//
        public string chy { get; set; }// 3 /chy { get; set; }//
        public string zho_shuoming { get; set; }// 暂无 /zho_shuoming { get; set; }//
        public string diy_shuoming { get; set; }// 暂无 /diy_shuoming { get; set; }//
        public string fas_shuoming { get; set; }// 暂无 /fas_shuoming { get; set; }//
        public string chy_shuoming { get; set; }// 单层薄衫、裤薄型棉衫、长裤、针织长袖衫、长袖T恤。薄型套装、牛仔衫裤、西服套装、薄型夹克 /chy_shuoming { get; set; }//
        public string pollution_l { get; set; }// 轻度 /pollution_l { get; set; }//
        public string zwx_l { get; set; }// 最弱 /zwx_l { get; set; }//
        public string ssd_l { get; set; }// 温暖舒适 /ssd_l { get; set; }//
        public string fas_l { get; set; }// 暂无 /fas_l { get; set; }//
        public string zho_l { get; set; }// 暂无 /zho_l { get; set; }//
        public string chy_l { get; set; }// 单衣类 /chy_l { get; set; }//
        public string ktk_l { get; set; }// 不需要开启 /ktk_l { get; set; }//
        public string xcz_l { get; set; }// 不太适宜 /xcz_l { get; set; }//
        public string diy_l { get; set; }// 暂无 /diy_l { get; set; }//
        public string pollution_s { get; set; }// 对空气污染物扩散无明显影响 /pollution_s { get; set; }//
        public string zwx_s { get; set; }// 紫外线最弱 /zwx_s { get; set; }//
        public string ssd_s { get; set; }// 天气状况良好时，多到户外活动，并可适当增加户外活动时间。 /ssd_s { get; set; }//
        public string ktk_s { get; set; }// 不需要开启空调 /ktk_s { get; set; }//
        public string xcz_s { get; set; }// 洗车后未来1-2天内有降水、大风或沙尘天气，不太适宜洗车 /xcz_s { get; set; }//
        public string gm { get; set; }// 1 /gm { get; set; }//
        public string gm_l { get; set; }// 低发期 /gm_l { get; set; }//
        public string gm_s { get; set; }// 天气舒适，不易发生感冒； /gm_s { get; set; }//
        public string yd { get; set; }// 3 /yd { get; set; }//
        public string yd_l { get; set; }// 适宜 /yd_l { get; set; }//
        public string yd_s { get; set; }// 天气较暖，比较适宜户外运动； /yd_s { get; set; }//
        public string savedate_weather { get; set; }// 2015-05-21 /savedate_weather { get; set; }//
        public string savedate_life { get; set; }// 2015-05-21 /savedate_life { get; set; }//
        public string savedate_zhishu { get; set; }// 2015-05-21 /savedate_zhishu { get; set; }//
        public string udatetime { get; set; }// 2015-05-21 08:10:00 /udatetime { get; set; }//
    }
}