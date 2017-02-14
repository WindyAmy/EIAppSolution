using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;

namespace EIApp.Common
{
    #region 应用程序配置类型枚举

    /// <summary>
    /// 应用程序配置类型枚举
    /// </summary>
    public enum ConfigurationType
    {
        /// <summary>
        /// 客户端应用程序
        /// </summary>
        App = 1,

        /// <summary>
        /// Web应用程序
        /// </summary>
        Web = 2
    }

    #endregion 应用程序配置类型枚举

    #region Xml的操作公共类

    /// <summary>
    /// Xml的操作公共类
    /// </summary>
    public class XmlHelper
    {
        #region 字段定义

        /// <summary>
        /// XML文件的物理路径
        /// </summary>
        private string _filePath = string.Empty;

        #endregion 字段定义

        #region 定义属性

        /// <summary>
        /// Xml文档
        /// </summary>
        private XmlDocument _xml;

        /// <summary>
        /// Xml文档
        /// </summary>
        public XmlDocument Xml
        {
            get { return _xml; }
        }

        /// <summary>
        /// XML的根节点
        /// </summary>
        private XmlElement _element;

        /// <summary>
        /// XML的根节点
        /// </summary>
        public XmlElement Element
        {
            get { return _element; }
        }

        #endregion 定义属性

        #region 构造方法

        /// <summary>
        /// 实例化XmlHelper对象
        /// </summary>
        /// <param name="xmlAbsolutePath">Xml文件的绝对路径</param>
        public XmlHelper(string xmlAbsolutePath)
        {
            _filePath = xmlAbsolutePath;
            CreateXMLElement();
        }

        /// <summary>
        /// 实例化XmlHelper对象
        /// </summary>
        /// <param name="configurationType">要操作的配置文件类型,枚举常量</param>
        /// <param name="xmlVirtualPath">Xml文件的相对路径</param>
        public XmlHelper(ConfigurationType configurationType, string xmlVirtualPath)
        {
            //获取XML文件的绝对路径
            _filePath = GetPath(configurationType, xmlVirtualPath);
            CreateXMLElement();
        }

        #endregion 构造方法

        #region 私有方法

        #region 获取XML的根节点

        /// <summary>
        /// 获取XML的根节点
        /// </summary>
        private void CreateXMLElement()
        {
            //创建一个XML对象
            _xml = new XmlDocument();

            if (File.Exists(_filePath))
            {
                //加载XML文件
                _xml.Load(this._filePath);
            }

            //为XML的根节点赋值
            _element = _xml.DocumentElement;
        }

        #endregion 获取XML的根节点

        #region 返回虚拟路径的绝对路径

        /// <summary>
        /// 返回虚拟路径的绝对路径
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        private string GetPath(ConfigurationType configurationType, string virtualPath)
        {
            return configurationType.Equals(ConfigurationType.Web) ?
                HttpContext.Current.Server.MapPath(virtualPath) :
                AppDomain.CurrentDomain.BaseDirectory + virtualPath;
        }

        #endregion 返回虚拟路径的绝对路径

        #endregion 私有方法

        #region 获取节点

        #region 获取指定XPath表达式的节点对象

        /// <summary>
        /// 获取指定XPath表达式的节点对象
        /// </summary>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public XmlNode GetNode(string xPath)
        {
            //创建XML的根节点
            //CreateXMLElement();

            //返回XPath节点
            return _element.SelectSingleNode(xPath);
        }

        /// <summary>
        /// 获取指定XPath表达式的节点对象
        /// </summary>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public XmlNodeList GetNodeList(string xPath)
        {
            //创建XML的根节点
            //CreateXMLElement();

            //返回XPath节点
            return _element.SelectNodes(xPath);
        }

        #endregion 获取指定XPath表达式的节点对象

        #region 获取指定XPath表达式节点的值

        /// <summary>
        /// 获取指定XPath表达式节点的值
        /// </summary>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public string GetValue(string xPath)
        {
            //创建XML的根节点
            //CreateXMLElement();

            //返回XPath节点的值
            return _element.SelectSingleNode(xPath).InnerText;
        }

        #endregion 获取指定XPath表达式节点的值

        #region 获取指定XPath表达式节点的属性值

        /// <summary>
        /// 获取指定XPath表达式节点的属性值
        /// </summary>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        /// <param name="attributeName">属性名</param>
        public string GetAttributeValue(string xPath, string attributeName)
        {
            //创建XML的根节点
            //CreateXMLElement();

            //返回XPath节点的属性值
            return _element.SelectSingleNode(xPath).Attributes[attributeName].Value;
        }

        #endregion 获取指定XPath表达式节点的属性值

        #endregion 获取节点

        #region 新增节点

        /// <summary>
        /// 1. 功能：新增节点。
        /// 2. 使用条件：将任意节点插入到当前Xml文件中。
        /// </summary>
        /// <param name="xmlNode">要插入的Xml节点</param>
        public void AppendNode(XmlNode xmlNode)
        {
            //创建XML的根节点
            //CreateXMLElement();

            //导入节点
            XmlNode node = _xml.ImportNode(xmlNode, true);

            //将节点插入到根节点下
            _element.AppendChild(node);
        }

        /// <summary>
        /// 1. 功能：新增节点。
        /// 2. 使用条件：将任意节点字符串插入到当前Xml文件中。
        /// </summary>
        /// <param name="name">节点名称</param>
        /// <param name="InnerXml">节点内部字符串</param>
        public void AppendNode(string name, string InnerXml)
        {
            //创建XML的根节点
            //CreateXMLElement();

            //导入节点
            XmlNode node = _xml.CreateElement(name);
            node.InnerXml = InnerXml;

            //将节点插入到根节点下
            _element.AppendChild(node);
        }

        #endregion 新增节点

        #region 删除节点

        /// <summary>
        /// 删除指定XPath表达式的节点
        /// </summary>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public void RemoveNode(string xPath)
        {
            //创建XML的根节点
            //CreateXMLElement();

            //获取要删除的节点
            XmlNode node = _xml.SelectSingleNode(xPath);

            //删除节点
            //_element.RemoveChild(node);
            node.ParentNode.RemoveChild(node);
        }

        #endregion 删除节点

        #region 编辑节点

        /// <summary>
        ///  根据指定XPath表达式节点，编辑属性
        /// </summary>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        /// <param name="dicAttribute">属性字典</param>
        public void EditElementAttribute(string xPath, Dictionary<string, string> dicAttribute)
        {
            XmlElement elmt = null;
            //获取要修改的节点
            XmlNode node = _xml.SelectSingleNode(xPath);
            if (node.NodeType != XmlNodeType.Element)
            {
                throw (new Exception("XmlNodeType is Element Can SetAttribute!"));
            }
            else
            {
                elmt = (XmlElement)node;
            }
            //根据Dictionary设置属性
            //遍历字典
            foreach (KeyValuePair<string, string> kvp in dicAttribute)
            {
                elmt.SetAttribute(kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        ///  根据指定XPath表达式节点修改内部文本
        /// </summary>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        /// <param name="innerText">内部文本</param>
        public void EditNodeText(string xPath, string innerText)
        {
            //获取要修改的节点
            XmlNode node = _xml.SelectSingleNode(xPath);

            node.InnerText = innerText;
        }

        /// <summary>
        ///  根据指定XPath表达式节点修改内部Xml
        /// </summary>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        /// <param name="innerText">内部Xml</param>
        public void EditNodeXml(string xPath, string innerXml)
        {
            //获取要修改的节点
            XmlNode node = _xml.SelectSingleNode(xPath);

            node.InnerXml = innerXml;
        }

        #endregion 编辑节点

        #region 保存XML文件

        /// <summary>
        /// 保存XML文件
        /// </summary>
        public void Save()
        {
            //创建XML的根节点
            //CreateXMLElement();

            //保存XML文件
            _xml.Save(this._filePath);
        }

        #endregion 保存XML文件

        #region 静态方法

        #region 创建根节点对象

        /// <summary>
        /// 创建根节点对象
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的绝对路径</param>
        private static XmlElement CreateRootElement(string xmlFilePath)
        {
            //定义变量，表示XML文件的绝对路径
            string filePath = "";

            //获取XML文件的绝对路径
            filePath = xmlFilePath;

            //创建XmlDocument对象
            XmlDocument xmlDocument = new XmlDocument();
            //加载XML文件
            xmlDocument.Load(filePath);

            //返回根节点
            return xmlDocument.DocumentElement;
        }

        #endregion 创建根节点对象

        #region 获取指定XPath表达式节点的值

        /// <summary>
        /// 获取指定XPath表达式节点的值
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的绝对路径</param>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        public static string GetValue(string xmlFilePath, string xPath)
        {
            //创建根对象
            XmlElement rootElement = CreateRootElement(xmlFilePath);

            //返回XPath节点的值
            return rootElement.SelectSingleNode(xPath).InnerText;
        }

        #endregion 获取指定XPath表达式节点的值

        #region 获取指定XPath表达式节点的属性值

        /// <summary>
        /// 获取指定XPath表达式节点的属性值
        /// </summary>
        /// <param name="xmlFilePath">Xml文件的绝对路径</param>
        /// <param name="xPath">XPath表达式,
        /// 范例1: @"Skill/First/SkillItem", 等效于 @"//Skill/First/SkillItem"
        /// 范例2: @"Table[USERNAME='a']" , []表示筛选,USERNAME是Table下的一个子节点.
        /// 范例3: @"ApplyPost/Item[@itemName='岗位编号']",@itemName是Item节点的属性.
        /// </param>
        /// <param name="attributeName">属性名</param>
        public static string GetAttributeValue(string xmlFilePath, string xPath, string attributeName)
        {
            //创建根对象
            XmlElement rootElement = CreateRootElement(xmlFilePath);

            //返回XPath节点的属性值
            return rootElement.SelectSingleNode(xPath).Attributes[attributeName].Value;
        }

        #endregion 获取指定XPath表达式节点的属性值

        #endregion 静态方法

        #region XmlDatasetConvert静态方法

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

        #endregion XmlDatasetConvert静态方法
    }

    #endregion Xml的操作公共类
}
