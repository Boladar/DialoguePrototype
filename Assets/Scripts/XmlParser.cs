using UnityEngine;
using System.Xml;
using System.Collections.Generic;
using System.IO;

public class XmlParser : MonoBehaviour {

	public Dictionary<string,Dialogue> Dialogues = new Dictionary<string, Dialogue>();
	public Dictionary<string, bool> Flags = new Dictionary<string, bool>();
	// Use this for initialization
	void Awake () {
		ReadDialoguesFromXML ("data");
		ReadFlagsFromXML ("Flags");
	}

	public void CreateDialogues(XmlReader reader){
		Dialogue currentDialog = new Dialogue();
		Dialogues.Add (reader.GetAttribute("id"), currentDialog); 

		XmlReader childReader = reader.ReadSubtree();

		while(childReader.Read())
		{
			if (reader.Name == "Condition") {
				childReader.Read ();
				currentDialog.Conditions.Add (childReader.ReadContentAsString ());
			}
			if(reader.Name == "Message"){
				Message m = new Message ();
				m.Speaker = childReader.GetAttribute ("id");
				m.AudioFilename = childReader.GetAttribute ("file");

				childReader.Read();

				m.Text = childReader.ReadContentAsString ();
				currentDialog.Messages.Add (m);

			}
			if(childReader.Name == "Option"){
				Option o = new Option();
				o.ID = childReader.GetAttribute ("id");

				childReader.Read();

				o.Text = childReader.ReadContentAsString ();
				currentDialog.Options.Add (o);
			}
			if (childReader.Name == "Trigger") {
				childReader.Read ();
				currentDialog.Triggers.Add (childReader.ReadContentAsString ());
			}
		}
	}

	public void ReadDialoguesFromXML(string filename)
	{
		TextAsset textAsset = (TextAsset)Resources.Load(filename);
		XmlTextReader reader = new XmlTextReader( new StringReader(textAsset.text));

		if (reader.ReadToDescendant ("Dialogues")) {
			if (reader.ReadToDescendant ("Dialogue")) {
				do{
					CreateDialogues(reader);
				}while(reader.ReadToNextSibling("Dialogue"));
			} else
				Debug.LogError ("Dialogue is missing!");
		} else
			Debug.LogError ("XML file does not containt Dialogues!");
	}

	public void ReadFlagsFromXML(string filename)
	{
		TextAsset textAsset = (TextAsset)Resources.Load (filename);
		XmlTextReader reader = new XmlTextReader( new StringReader(textAsset.text));

		if (reader.ReadToDescendant ("Flags")) {
			if (reader.ReadToDescendant ("Flag")) {
				do {

					string name = reader.GetAttribute("name");
					reader.Read();
					bool value = reader.ReadContentAsBoolean();

					Flags.Add(name,value);

				} while(reader.ReadToNextSibling ("Flag"));
			}
		}
	}

}

public class Dialogue {
	public string ID{ get; set; }
	public List<string> Conditions { get; set;}
	public List<string> Triggers {get;set;}
	public List<Message> Messages{ get; set; }
	public List<Option> Options{ get; set; }

	public Dialogue(){
		Conditions = new List<string> ();
		Triggers = new List<string> ();
		Options = new List<Option> ();
		Messages = new List<Message> ();
	}
}

public class Message{
	public string Speaker{ get; set; }
	public string Text {get;set;}
	public string AudioFilename{ get; set;}
}

public class Option{
	public string ID{ get; set; }
	public string Text{ get; set; }
}