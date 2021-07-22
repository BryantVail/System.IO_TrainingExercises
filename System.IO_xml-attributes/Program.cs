using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Healthcare.Prescriptions.VendorObjects.MdScripts;
using PrescriptionsClassLibrary.VendorObjects.MdScripts;

namespace System.IO_xml_attributes
{

	// test class
	public class ClassName
	{
		// change xml Attribute name
		[XmlAttribute(AttributeName = "Doctor")]
		public string Prescriber { get; set; }

		[XmlAttribute(AttributeName = "Prescriber")]
		public string Doctor { get; set; }

	}

	class Program
	{
		static void Main(string[] args) {


			var xmlProcessor = new PatientXmlProcessor();
			var exeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
			string strWorkingDirectory = System.IO.Path.GetDirectoryName(exeFilePath);

			xmlProcessor.DeserializeObject(
				$"{strWorkingDirectory}..\\..\\..\\..\\data\\mdScriptsPrescriptionWithPrescriber.xml");
			xmlProcessor.DeserializeObject(
				$"{strWorkingDirectory}..\\..\\..\\..\\data\\mdScriptsPrescriptionWithDoctor.xml");


		}// end main



		

	}// end Program.cs

	public interface IXmlProcessor<TranslateFromObject>
	{
		string SerializeObject(string filePath);
		string DeserializeObject(string filePath);
	}

	public class PatientXmlProcessor : IXmlProcessor<Patient>
	{
		public string SerializeObject(string fileName) {
			Console.WriteLine("Serializing");

			XmlSerializer mySerializer = new XmlSerializer(typeof(Prescription));

			TextWriter textWriter = new StreamWriter(fileName);

			Prescription myGroup = new Prescription();
			//myGroup.Prescriber = "Prescriber";
			//myGroup.Doctor = "Doctor";

			//mySerializer.Serialize(textWriter, myGroup);
			textWriter.Close();

			return new string("hello world");
		}

		public string DeserializeObject(string xmlFilePath) {

			Console.WriteLine($"Deserializing...");

			var types = new List<Type>();
			types.Add(typeof(Order[]));
			types.Add(typeof(OrderItem[]));
			types.Add(typeof(PatientDescriptor));
			types.Add(typeof(Prescriber));
			types.Add(typeof(Prescription[]));
			types.Add(typeof(EPrescription[]));
			types.Add(typeof(PrescribedItem[]));
			types.Add(typeof(DispensedItem[]));
			types.Add(typeof(List<string>));
			types.Add(typeof(Guid));
			types.Add(typeof(DateTime?));
			types.Add(typeof(DateTime));
			types.Add(typeof(PrescriptionsClassLibrary.VendorObjects.MdScripts.Site));


			XmlSerializer mySerializer =
				new XmlSerializer(typeof(Patient), types.ToArray());

				FileStream fileStream =
					new FileStream(xmlFilePath, FileMode.Open);
			//XmlReader xmlReader = new XmlReader();

			Patient patientWithPrescriptions = (Patient)mySerializer.Deserialize(fileStream);

				string returnValue = "";
				int iterator = 0;
				foreach (var prescription in patientWithPrescriptions.Prescriptions)
				{
					if (iterator == 0)
					{
						returnValue += $"[\n";
					}

					returnValue += $"\t{{\n" +
						$"\t\t\"Prescriber\":\t\"{prescription.Prescriber}\",\n" +
						//$"\t\t\"Doctor\":\t\t\"{prescription.Doctor}\"\"" +
						$"\t}}";

					if (iterator != patientWithPrescriptions.Prescriptions.Length - 1)
					{
						returnValue += ",\n";
					}
					else
					{
						returnValue += $"\n]";
					}


				}// end foreach

				Console.WriteLine(returnValue);
				return returnValue;
		}// end deserializeObject
	}
}
