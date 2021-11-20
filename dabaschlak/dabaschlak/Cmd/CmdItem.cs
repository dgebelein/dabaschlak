using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace dabaschlak
{
public enum CmdResponse
{
	None,
	ZeigeAllePersonen,
	ZeigeAlleFlaechen,
	ZeigeAlleVersuche,
	VersuchsArbeitenDokumentieren,
	FlaechenArbeitenDokumentieren,
	Nachrichtenzuordnung,
	MeineNachrichten,
	LetzteVersuchsplaene,
	Browser,
	ExternFile,
	FlexItem,
	Versuchsprotokoll,
	Abfrage
};


	public class CmdItem: CmdBase
	{
		//public string Text { get; set; }
		//public Brush TextColor { get; set; }
		//public int Heigth { get; set; }
		public CmdResponse Response { get; set; }
		public string Url { get; set; }
		


	}
}
