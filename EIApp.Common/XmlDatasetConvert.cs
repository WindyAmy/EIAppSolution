using System.Data;
using System.IO;
using System.Text;
using System.Xml;

namespace EIApp.Common
{
    public class XmlDatasetConvert
    {
        #region 将xml字符串转换为DataSet

        /// <summary>
        /// 将xml对象内容字符串转换为DataSet
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static DataSet XmlString2DataSet(string xmlData)
        {
            StringReader stream = null;
            XmlTextReader reader = null;
            DataSet xmlDS = null;
            try
            {
                xmlDS = new DataSet();
                using (stream = new StringReader(xmlData))
                {
                    //从stream装载到XmlTextReader
                    using (reader = new XmlTextReader(stream))
                    {
                        xmlDS.ReadXml(reader);
                    }
                }
                return xmlDS;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        #endregion 将xml字符串转换为DataSet

        #region 将xml文件转换为DataSet

        /// <summary>
        /// 将xml文件转换为DataSet
        /// </summary>
        /// <param name="xmlFile">文件路径</param>
        /// <returns></returns>
        public static DataSet XmlFile2DataSet(string xmlFile)
        {
            XmlTextReader reader = null;
            try
            {
                DataSet xmlDS = new DataSet();

                //从stream装载到XmlTextReader
                using (reader = new XmlTextReader(xmlFile))
                {
                    xmlDS.ReadXml(reader);
                }
                //xmlDS.ReadXml(xmlFile);
                return xmlDS;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        #endregion 将xml文件转换为DataSet

        #region 将DataSet转换为xml对象字符串

        /// <summary>
        ///  将DataSet转换为xml对象字符串
        /// </summary>
        /// <param name="xmlDS"></param>
        /// <returns></returns>
        public static string DataSet2XmlString(DataSet xmlDS)
        {
            MemoryStream stream = null;
            XmlTextWriter writer = null;

            try
            {
                stream = new MemoryStream();
                //从stream装载到XmlTextReader
                //writer = new XmlTextWriter(stream, Encoding.Unicode);//Unicode有点问题，可能是字符集不一致
                writer = new XmlTextWriter(stream, Encoding.Default);

                //用WriteXml方法写入文件.
                xmlDS.WriteXml(writer);
                int count = (int)stream.Length;
                byte[] arr = new byte[count];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(arr, 0, count);

                //UnicodeEncoding utf = new UnicodeEncoding();
                //return utf.GetString(arr).Trim();
                return Encoding.Default.GetString(arr).Trim();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (writer != null) writer.Close();
            }
        }

        #endregion 将DataSet转换为xml对象字符串

        #region 将DataSet转换为xml文件

        /// <summary>
        /// 将DataSet转换为xml文件
        /// </summary>
        /// <param name="xmlDS"></param>
        /// <param name="xmlFile"></param>
        public static void DataSet2XmlFile(DataSet xmlDS, string xmlFile)
        {
            try
            {
                xmlDS.WriteXml(xmlFile);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            #region 复杂实现方法

            //MemoryStream stream = null;
            //XmlTextWriter writer = null;

            //try
            //{
            //    stream = new MemoryStream();
            //    //从stream装载到XmlTextReader
            //    //writer = new XmlTextWriter(stream, Encoding.Unicode);
            //    writer = new XmlTextWriter(stream, Encoding.Default);

            //    //用WriteXml方法写入文件.
            //    xmlDS.WriteXml(writer);
            //    int count = (int)stream.Length;
            //    byte[] arr = new byte[count];
            //    stream.Seek(0, SeekOrigin.Begin);
            //    stream.Read(arr, 0, count);

            //    //返回Unicode编码的文本
            //    //UnicodeEncoding utf = new UnicodeEncoding();
            //    StreamWriter sw = new StreamWriter(xmlFile);
            //    sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            //    sw.WriteLine(Encoding.Default.GetString(arr).Trim());
            //    sw.Close();
            //}
            //catch (System.Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    if (writer != null) writer.Close();
            //}

            #endregion 复杂实现方法
        }

        #endregion 将DataSet转换为xml文件
    }
}