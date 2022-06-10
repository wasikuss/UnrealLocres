using XlfParser.Model;

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnrealLocres.Converter
{
    public sealed class XliffConverter : BaseConverter
    {
        public override string ExportExtension => "xliff";

        public override string ImportExtension => "xliff";

        protected override List<TranslationEntry> Read(TextReader reader)
        {
            var xliff = XlfParser.Converter.Deserialize(reader.ReadToEnd());
            return xliff.File.Body.TransUnit.Select(e => new TranslationEntry(e.Id, e.Source, e.Target)).ToList();
        }

        protected override void Write(List<TranslationEntry> data, TextWriter writer)
        {
            var xliff = new Xliff()
            {
                Version = 1.2m,
                File =  new XlfParser.Model.File() {
                    Datatype = "xml",
                    SourceLanguage = "lang1",
                    TargetLanguage = "lang2",
                    ToolId = "uelocres",
                    Header = new Header() {
                            Tool = new Tool() {
                                Id = "uelocres",
                                Company = "akintos",
                                Name = "UnrealLocres"
                            }
                    },
                    Body = new Body() {
                        TransUnit = data.Select(e => new TransUnit() {Id = e.Key, Source = e.Source, Target = e.Target}).ToList()
                    }
                }
            };
            writer.Write(XlfParser.Converter.Serialize(xliff));
        }
    }
}
