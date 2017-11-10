using System;
using System.Xml;
using System.Collections.Generic;

class HelloWorldWin {
    static void Main() {
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.IgnoreWhitespace = true;
        var d = new Dictionary<int, String>();
        using (XmlReader reader = XmlReader.Create("items.xml", settings)) {
            reader.MoveToContent();
            while(reader.Read()) {
                if (reader.NodeType == XmlNodeType.Element) {
                    switch(reader.Name) {
                        case "bounds":
                            //attributes: minlat minlon maxlat maxlon
                            Console.WriteLine(reader["minlat"]);
                            break;

                        case "node":
                            //attributes: id lat lon
                            //child: tag attributes: k v
                            break;

                        case "way":
                            //attributes: id
                            //child: tag attributes: k v
                            //child: nd attributes: ref -> node id
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    }
}
