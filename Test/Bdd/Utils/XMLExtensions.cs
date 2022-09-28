// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XMLExtensions.cs" company="Microsoft">
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//    THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//    OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//    OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Ecp.True.Bdd.Tests.Utils
 {
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Serialization;

    public static class XmlExtensions
    {
        public static T DeserializeArray<T>(this XmlNode[] nodes)
        {
            T result = default(T);
#pragma warning disable CA3075 // Insecure DTD processing in XML
            var doc = new XmlDocument();
            doc.LoadXml("<root/>");
#pragma warning restore CA3075 // Insecure DTD processing in XML
            nodes.ToList().ForEach(x =>
            {
                var n = doc.ImportNode(x, true);
                doc.DocumentElement.AppendChild(n);
            });
            using (var reader = XmlReader.Create(new StringReader(doc.OuterXml), new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit }))
            {
                try
                {
                    var ser = new XmlSerializer(typeof(T), new XmlRootAttribute(reader.Name));
                    result = (T)ser.Deserialize(reader);
                }
                catch (InvalidOperationException) //// Due to not providing root-name
                {
                    var ser = new XmlSerializer(typeof(T), new XmlRootAttribute(reader.Name) { IsNullable = true });
                    result = (T)ser.Deserialize(reader);
                }
            }

            return result;
        }
    }
}