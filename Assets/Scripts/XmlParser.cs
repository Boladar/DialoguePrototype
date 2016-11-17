using UnityEngine;
using System.Xml;
using System.Collections.Generic;
using System.IO;

public class XmlParser : MonoBehaviour {

	public Dictionary<string,Dialogue> Dialogues = new Dictionary<string, Dialogue>();

	// Use this for initialization
	void Awake () {
		ReadXml ("data");
	}

	public void CreateDialogues(XmlReader reader){
		Dialogue currentDialog = new Dialogue();
		Dialogues.Add (reader.GetAttribute("id"), currentDialog); 

		XmlReader childReader = reader.ReadSubtree();

		while(childReader.Read())
		{
			if(reader.Name == "Text")
			{
				childReader.Read();
				currentDialog.Text = childReader.ReadContentAsString ();
			}
			if(childReader.Name == "Option")
			{
				Option o = new Option();
				o.ID = childReader.GetAttribute ("id");

				childReader.Read();

				o.Text = childReader.ReadContentAsString ();
				currentDialog.Options.Add (o);
			}
		}
	}

	public void ReadXml(string name)
	{
		TextAsset textAsset = (TextAsset)Resources.Load(name);
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

}

public class Dialogue {
	public string ID{ get; set; }
	public string Text{ get; set; }
	public List<Option> Options{ get; set; }

	public Dialogue(){
		Options = new List<Option> ();
	}
}

public class Option{
	public string ID{ get; set; }
	public string Text{ get; set; }
}