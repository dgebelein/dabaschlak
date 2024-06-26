SQLite format 3   @    �                                                            � -�� 	��p$�
B��00�����                                                                                                                                                                                                                    �Y<++�mviewversuchsabfrageversuchsabfrageCREATE VIEW versuchsabfrage AS SELECT 
v.VersuchId, 
v.VerBez, 
Personen.Name || ', ' || Personen.Vorname AS Versuchsleiter, 
v.Start, 
v.Ende,
v.Versuchsplan, 
v.Versuchsfrage, 
GROUP_CONCAT(x.Fid) As standorte,
GROUP_CONCAT(f.FlaeBez) As standortbez
FROM Versuche v 
LEFT JOIN Personen ON v.Leiter = Personen.PersonId
LEFT JOIN VxF x ON v.VersuchId = x.Vid
Left Join Flaechen f on f.FlaeId= x.Fid
where v.Start=2015
Group by x.VidQ�ItableVersucheVersucheCREATE TABLE Versuche (VersuchId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, Leiter INTEGER REFERENCES Personen (PersonId), Start INTEGER NOT NULL, Ende INTEGER, VerBez TEXT NOT NULL, Versuchsplan TEXT, VersuchsfraN     �C@--�=viewMeineNachrichtenMeineNachrichtenCREATE VIEW MeineNachrichten AS SELECT Datum, Aktion, Notizen, v.VerBez, f.FlaeBez, a.Versuch FROM Aktionen a JOIN Versuche v ON a.Versuch = v.VersuchId JOIN Flaechen f ON f.FlaeId = a.Flaeche JOIN Nachrichten n WHERE n.VersuchId = a.Versuch and n.PersonId = 5 ORDER BY Datum DESC�-))�yviewrepnachrichtenrepnachrichtenCREATE VIEW repnachrichten AS Select  VersuchId,VerBez, f.FlaeBez, v.Start,v.Ende,
EXISTS (Select 1 from Nachrichten  n
    Where n.VersuchId =v.VersuchId  AND n.PersonId=1) as doNotify

from Versuche v
Left Join Flaechen f
On f.FlaeId= v.Flaeche �##H     �G;;�SM;;�=tablesqlitestudio_temp_tablesqlitestudio_temp_tableCREATE TABLE sqlitestudio_temp_table(
  VersuchId INT,
  Leiter INT,
  Start INT,
  Ende INT,
  VerBez TEXT,
  Versuchsplan TEXT,
  Versuchsfrage TEXT
)�=//�=tableArbeitskategorienArbeitskategorienCREATE TABLE Arbeitskategorien (Kategorie STRING, hatNotizen BOOLEAN, FehlerText STRING)� -�/viewarbeiten1arbeiten1CREATP     ^A�tableAdminsAdminsCREATE TABLE Admins (PersonId INTEGER REFERENCES Personen (PersonId))f�viewarbeiten1arbeiten1CREATE VIEW arbeiten1 AS SELECT a.Datum, a.Person, a.Aktion, a.Notizen, p.Name || ', ' || p.Vorname AS PersonName, f.FlaeBez, v.VerBez, v.Star�5O;;�tablesqlitestudio_temp_tablesqlitestudio_temp_tableCREATE TABLE sqlitestudio_temp_table(
  PersonId INT,
  Name TEXT,
  Vorname TEXT,
  Tel TEXT,
  Email TEXT,
  Aktiv NUM
)�>�tableAktionenAktionen
CREATE TABLE Aktionen (ArbeitId INTEGER PRIMARY KEY AUTOINCREMENT, Versuch INTEGER REFERENCES Versuche (VersuchId), Flaeche INTEGER REFERENCES Flaechen (FlaeId), Person INTEGER REFERENCES Personen (PersonId), Datum DATETIME, Aktion TEXT, Notizen TEXT){9�]tableVxFVxF	CREATE TABLE VxF (Vid INTEGER REFERENCES Versuche (VersuchId), Fid INTEGER REFERENCES Flaechen (FlaeId)) �IL##�YtableNachrichtenNachrichtenCREATE TABLE Nachrichten (Id INTEGER PRIMARY KEY, PersonId INTEGER REFERENCES Personen (PersonId), VersuchId INTEGER REFERENCES Versuche (VersuchId), PerMail BOOLEAN)��stableFlaechenFlaechenCREATE TABLE Flaechen (FlaeId INTEGER PRIMARY KEY AUTOINCREMENT, FlaeBez TEXT NOT NULL, FlaeTyp TEXT, FlaeEig TEXT)N -;;�Qta�O7;;�5�E:;;�!tablesqlitest�>P�OtablePersonenPersonenCREATE TABLE Personen (PersonId INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, Vorname TEXT, Tel TEXT, Email TEXT, Aktiv BOOLEAN NOT NULL, Netzname TEXT)�N�ctableVersucheVersucheCREATE TABLE Versuche (VersuchId INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, Leiter INTEGER REFERENCES Personen (PersonId), Start INTEGER NOT NULL, Ende INTEGER, VerBez TEXT NOT NULL, Versuchsplan TEXT, Versuchsfrage TEXT, Kultur TEXT)P++Ytablesqlite_sequencesqlite_sequenceCREATE TABLE sqlite_sequence(name,seq)   ��1tablePersonenPersonenCREATE TABLE Personen (PersonId INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT NOT NULL, Vorname TEXT, Tel TEXT, Email TEXT, Aktiv BOOLEAN NOT NULL) /� ��G��]"��p1���G���l5
�
�
f
,	�	�		N	��f+���I��]��W                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            > #O	KoeA K	GebeleinDieter4434dieter.gebelein@julius-kuehn.degebelein53 G	 WilkeAndreas4435andreas.wilke@julius-kuehn.deG2 -Y	 Rempe-VespermannNelli4451nelli.rempe-vespermann@julius-kuehn.de71 I	 WescheJohanna4405johanna.wesche@julius-kuehn.de<0 G	 Wagner_sStefan4411/4449stefan.wagner@julius-kuehn.deD/ Q	 TrautmannReinhold4616/4617reinhold.trautmann@julius-kuehn.de;. M	 TrautmannDagmar4608dagmar.trautmann@julius-kuehn.de7- I	 ThieleHenning4616henning.thiele@julius-kuehn.de9, !K	 StreithoffElke4653elke.streithoff@julius-kuehn.de9+ K	 StraussKirsten4446kirsten.strauss@julius-kuehn.de5* G	 SmolkaSilvia4422silvia.smolka@julius-kuehn.de3) E	 SevenJasmin4454jasmin.seven@julius-kuehn.de9( K	 SchroeterAchim4435achim.schroeter@julius-kuehn.de9' K	 SchorppQuentin4452quentin.schorpp@julius-kuehn.de9& #K	 ScheidemannUta4621uta.scheidemann@julius-kuehn.de;% M	 SchamlottSabine4414sabine.schamlott@julius-kuehn.de5$ G	 RoggeKerstin4447kerstin.rogge@julius-kuehn.de9# K	 PfaffAlexander4412alexander.pfaff@julius-kuehn.de7" I	 MitschkePetra4412petra.mitschke@julius-kuehn.de/! A	 MerkerIna4405ina.merker@julius-kuehn.de?  !Q	 LehmhusChristiane4404christiane.lehmhus@julius-kuehn.de5 G	 KuehneBianca4612bianca.kuehne@julius-kuehn.de3 E	 KrenzMarion4613marion.krenz@julius-kuehn.de8 I	 KoerberBaerbel4415barbel.koerber@julius-kuehn.de? #O	 Koennecke_fKerstin4446kerstin.koennecke@julius-kuehn.de? #Q	 KirchhammerKatrin4419katrin.kirchhammer@julius-kuehn.deK /]	 Karolczak-KlekampMarion4416marion.karolczak-klekamp@julius-kuehn.de5 G	 JunkerCorina4429corina.junker@julius-kuehn.de> !K	 JeworutzkiElke4435/4436elke.jeworutzki@julius-kuehn.de1 C	 IdczakElke4408elke.idczak@julius-kuehn.de5 G	 HommesMartin4400martin.hommes@julius-kuehn.de� 1A	 HoeferUte4435ute.hoefer@julius-kuehn.de7 I	 HerbstMalaika4424malaika.herbst@julius-kuehn.deE -W	 Heinrich-SiebersElke4410elke.heinrich-siebers@julius-kuehn.de3 E	 HauffeJulia4420julia.hauffe@julius-kuehn.de3 E	 GoetzMonika4403monika.goetz@julius-kuehn.de= O	 GottfriedHenrike4417henrike.gottfried@julius-kuehn.de7 I	 FoersterAntje4612antje.foerster@julius-kuehn.de< I	 FeldmannFalko4406/3213falko.feldmann@julius-kuehn.de9 K	 ErhardMichaela4614micheala.erhard@julius-kuehn.de9 K	 DresslerElvira4413elvira.dressler@julius-kuehn.de7 I	 DrechslerTina4426tina.drechsler@julius-kuehn.de7
 I	 BurlakKathrin4426kathrin.burlak@julius-kuehn.de;	 M	 BraesickeNadine4602nadine.braesicke@julius-kuehn.de9 K	 BoeckmannElias4441Elias.Boeckmann@julius-kuehn.deA !S	 BerendesKarl-Heinz4605karl-heinz.berendes@julius-kuehn.de9 K	 AchillesDoerte4426doerte.achilles@julius-kuehn.de   5 A	HoeferUte4435ute.hoefer@julius-kuehn.dehoefer� � �����                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        Aktionen Aktionen �	Flaechen �Versuche2   Personen3Personen3   �    ���	�Xr�S��� �                                                                                                                                                  �p A�Q�E��Prognose Kohlfliege Satz 3\\Bs-fs03\gf\12 - Schlagkartei\2017_Versuchspläne\AG Hommes\VP_Brokkoli_Prognose KF_2017 Hoe.docxPrognose Kleine Kohlfliege mittels Eimanschetten/
Bekämpfung von Kohldrehherzgallmücke 
Brokkoli�p A�Q�E��Prognose Kohlfliege Satz 2\\Bs-fs03\gf\12 - Schlagkartei\2017_Versuchspläne\AG Hommes\VP_Brokkoli_Prognose KF_2017 Hoe.docxPrognose Kleine Kohlfliege mittels Eimanschetten/
Bekämpfung von Kohldrehherzgallmücke 
Brokkoli�p A�Q�E��Prognose Kohlfliege Satz 1\\Bs-fs03\gf\12 - Schlagkartei\2017_Versuchspläne\AG Hommes\VP_Brokkoli_Prognose KF_2017 Hoe.docxPrognose Kleine Kohlfliege mittels Eimanschetten/
Bekämpfung von Kohldrehherzgallmücke 
Brokkoli�?
 W�[G��Kohlfliegenbekämpfung Rettich Satz 2\\Bs-fs03\gf\12 - Schlagkartei\2017_Versuchspläne\AG Hommes\Skizze_VP_Rettich_KF_2017_BS_Satz_1+2.docxBekämpfung Kleine KohlfliegeRettich�?
 W�[G��Kohlfliegenbekämpfung Rettich Satz 1\\Bs-fs03\gf\12 - Schlagkartei\2017_Versuchspläne\AG Hommes\Skizze_VP_Rettich_KF_2017_BS_Satz_1+2.docxBekämpfung Kleine KohlfliegeRettich�N
 K�M{��Kohlfiegenbekämpfung Chinakohl\\Bs-fs03\gf\12 - Schlagkartei\2017_Versuchspläne\AG Hommes\Skizze_VP_Chinakohl_KF_2017 BS.docxBekämpfung Kleine Kohlfliege, Erdfloh u.a. SchädlingeChinakohl�	 ��i�#��Wirksamkeit versch. Insektizide gegen Thrips an Brachyscome\\Bs-fs03\gf\12 - Schlagkartei\2017_Versuchspläne\AG Böckmann\VP_Skizze_Wirksamkeit Thrips Brachyscome .docxBiologische Wirksamkeit verschiedener Insektizide sowie Wirkungssteigerung durch Zuckerzusatz zur Bekämpfung von
Frankliniella occidentalis an Brachyscome im Gewächshaus
- Ringversuch 2017 -
Brachyscome�c g�Q���Bekämpfung Mottenschildlaus durch E. formosa\\Bs-fs03\gf\12 - Schlagkartei\2017_Versuchspläne\AG Böckmann\VP_Skizze KMSL_EFormosa_23_02.docxBekämpfung der Kohlmottenschildlaus durch Encarsia formosaRosenkohl�d k��37��Applikationstechnik Thrips Brachyscome Geranien\\Bs-fs03\gf\12 - Schlagkartei\2017_Versuchspläne\AG Böckmann\VP_Skizze_Applikationstechnik Thrips Brachyscome Geranie.docxPSM-Anlagerung bei Verwendung unterschiedlicher Düsentypen 
bei Breit- und Schmalblättrigen Zierpflanzen
- Applikationstechnik Versuch 2017 -
Brachyscome, Geranien�R
 I�[�{#��Rostmilbe an Tomaten Frühjahr\\Bs-fs03\gf\12 - Schlagkartei\2017_Versuchspläne\AG Böckmann\VP-Skizze Rostmilbe Frühjahr 2017.docxIst Rostmilbenbefall an Tomatenpflanzen mit spektrometrischen Messungen unabhängig von der Wasserversorgung der Pflanzen detektierbar, bevor dieser mit dem bloßen Auge erkannt wird?Tomate�	 m�o�+;��Reproduktionsrate von Diaretiella Rapae NoChoice\\Bs-fs03\gf\12 - Schlagkartei\2017_Versuchspläne\AG Böckmann\VP_Skizze_KS_Diaretiella_NoChoice_18_01_2017.docxReproduktionsrate von Diaretiella Rapae auf Rhopalosiphum padi und Mehliger KBLRosenkohl, Winterweizen� k�k�+;��Reproduktionsrate von Diaretiella Rapae  Choice\\Bs-fs03\gf\12 - Schlagkartei\2017_Versuchspläne\AG Böckmann\VP_Skizze_KS_Diaretiella_Choice_28_02_2017.docxReproduktionsrate von Diaretiella Rapae auf Rhopalosiphum padi und Mehliger KBLRosenkohl, Winterweizen�. C�a�;��Kohlblattlaus mit Untersaat\\Bs-fs03\gf\12 - Schlagkartei\2017_Versuchspläne\AG Böckmann\VP_Skizze Mehlige Kohlblattlaus_23_02.docxBekämpfung der mehligen Kohlblattlaus durch Untersaat/Banker Plant System mit Getreideblattläusen für den Parasitoiden D. rapaeWeißkohl, Winterweizen�W Q�9�)2��Schlupfversuch Porree-Minierfliege\\Bs-fs03\gf\12 - Schlagkartei\2017_Versuchspläne\AG Hommes\Versuch-2017-Schlupf.docxWie hängt die Schlupfrate der Porreeminierfliege mit der Bodentiefe zusammen?-�-
 Y�5G��Rettich Kohlfliegenbekämpfung  Satz 1\\Bs-fs03\gf\12 - Schlagkartei\2017_Versuchspläne\AG Hommes\VP_Rettich_GH_2017.docxBekämpfung Kleine Ko   1         �    ���|fP:#�����x]B(�����n]L;*��������o^M;)�������xgVE4#
�
�
�
�
�
�
�
y
h
W
F
5
$

	�	�	�	�	�	�	�	z	k	\	DF	"		 �������xgV�h%��z\��"��4��S�a���                                                            ]e �'GH20 K111KTemperaturregelung, Feuchteregelung
Stellfläche: Tische
AssimilationslichtJ� �C K156KInsektenzuchten Forst
Temperaturregelung
Leuchtstoffröhren!� 7C K061KLagerraum
kein LichtH�
 �C K060KTemperaturregelung
Stellfläche: Tisch
AssimilationslichtH�	 �C K059KTemperaturregelung
Stellfläche: Tisch
AssimilationslichtY� �%C K057KQuarantäneraum
Temperaturregelung
Stellfläche: Tisch
AssimilationslichtH� �C K050KTemperaturregelung
Stellfläche: Tisch
AssimilationslichtH� �C K049KTemperaturregelung
Stellfläche: Tisch
AssimilationslichtH� �C K048KTemperaturregelung
Stellfläche: Tisch
AssimilationslichtH� �C K047KTemperaturregelung
Stellfläche: Tisch
AssimilationslichtK� �	B K311KTageslicht-Zuchtraum
Temperaturregelung
(Assimilationslicht)@� uB K302KTemperaturregelung
Stellfläche: Regale
kein LichtZ� �'B K301KTemperaturregelung, Feuchteregelung
Stellfläche: Regale
LeuchtstoffröhrenZ�  �'B K202KTemperaturregelung, Feuchteregelung
Stellfläche: Regale
Leuchtstoffröhren  B K201K8~ eB K102KPilzkulturen
Temperaturregelung
kein Licht8} eB K101KPilzkulturen
Temperaturregelung
kein LichtY| �%B K112KTemperaturregelung, Feuchteregelung
Stellfläche: Tisch
Assimilationslicht{ -B K003KThermostatenraumz -B K001KThermostatenraumIy �A K168KTemperaturregelung
Stellfläche: Regale
Leuchtstoffröhren@x uA K167KTemperaturregelung
Stellfläche: Regale
ohne LichtAw uA K144bKTemperaturregelung
Stellfläche: Regale
ohne Licht^v �-A K144aKnur für niedrige Temperaturen (keine Heizung)
Stellfläche: Regale
ohne LichtEu A K032KInsektenzuchtraum
Temperaturregelung
LeuchtstoffröhrenEt A K031KInsektenzuchtraum
Temperaturregelung
Leuchtstoffröhrens  GH21 K110Kr  GH21 K109Kq  GH21 K108Kp  GH21 K010Ko  GH21 K009Kn  GH20 K123Km  GH20 K122Kl  GH20 K121Kk  GH20 K117Kj  GH20 K116Ki  GH20 K115Kh  GH20 K114Kg  GH20 K113K    GH20 K112Kd Ahlum 4A23 x 30 mc  GH20 C8Gb  GH20 C7Ga  GH20 C6-4G`  GH20 C6-3G_  GH20 C6-2G^  GH20 C6-1G]  GH20 C5-4G\  GH20 C5-3G[  GH20 C5-2GZ  GH20 C5-1GY  GH20 C4-4GX  GH20 C4-3GW  GH20 C4-2GV  GH20 C4-1GU  GH20 C3-9GT  GH20 C3-8GS  GH20 C3-7GR  GH20 C3-6GQ  GH20 C3-5GP  GH20 C3-4GO  GH20 C3-3GN  GH20 C3-2GM  GH20 C3-1GL  GH20 C2-3GK  GH20 C2-2GJ  GH20 C2-1GI  GH20 C1-4GH  GH20 C1-3GG  GH20 C1-2GF GH20 C1-1GE  GH21 B6-5GD  GH21 B6-4GC  GH21 B6-3GB  GH21 B6-2GA  GH21 B6-1G@ ! GH21 B5-18G? ! GH21 B5-17G> ! GH21 B5-16G= ! GH21 B5-15G< ! GH21 B5-14G; ! GH21 B5-13G: ! GH21 B5-12G9 ! GH21 B5-11G8 ! GH21 B5-10G7  GH21 B5-9G6  GH21 B5-8G5  GH21 B5-7G4  GH21 B5-6G3  GH21 B5-5G2  GH21 B5-4G1  GH21 B5-3G0  GH21 B5-2G/  GH21 B5-1G.  GH21 B4-2G-  GH21 B4-1G,  GH21 B3-2G+  GH21 B3-1G*  GH21 B2-2G)  GH21 B2-1G(  GH21 B1-2G'  GH21 B1-1G& 'BS 9.4Aca. 24 x 45 m% 'BS 9.3Aca. 24 x 45 m$ 'BS 9.2Aca. 24 x 45 m# 'BS 9.1Aca. 24 x 45 m" %BS 8.4Aca 35 x 50 m! %BS 8.3Aca 35 x 50 m  %BS 8.2Aca 35 x 50 m %BS 8.1Aca 35 x 50 m 'BS 7.4Aca. 20 x 40 m 'BS 7.3Aca. 20 x 40 m 'BS 7.2Aca. 20 x 40 m 'BS 7.1Aca. 20 x 40 m Ahlum 3A23 x 30 m Ahlum 2A23 x 30 m Ahlum 1A23 x 30 m Hö 11A13 x 40 m Hö 0A12 x 40 m Hö 10A27 x 40 m Hö 9A27 x 40 m Hö 8A27 x 40 m Hö 7A27 x 40 m Hö 6A27 x 40 m Hö 5A27 x 40 m Hö 4A27 x 40 m Hö 3A27 x 40 m Hö 2A27 x 40 m Hö 1   r   M   � ���_7]]]]]]]]]                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             }	 	C1�2016-11-24 13:54:43.1774331Herbizidbehandlungmit gift
und galle
und einer dritten zeile mit ganz viel buvjstaben= 	3=2016-11-28 00:00:00Sonstigesbla bla
oder auch nicht& 	3	2016-12-05 00:00:00Anzuchtmix� & 	3 2016-12-07 00:00:00Drillsaat  {	3% 2016-12-05 00:00:00Versuchsende  P	3)2016-12-05 00:00:00Pflanzenschutzgift   	32016-12-05 00:00:00HackenMist  �	3 	2016-11-30 00:00:00Hacken  �	3 2016-11-04 00:00:00Hacken� 83-2016-11-03 00:00:00Bodenbearbeitungsammmeln[) 	3%2016-11-30 00:00:00Versuchsende  H	C9
 3-#2016-11-03 00:00:00Bodenbearbeitungäöünbjko  312016-11-02 00:00:00Anzuchtaussaat in pikibox & 		32016-12-15 00:00:00Düngungmmm( 	32016-12-14 00:00:00Düngungmist& 3 2016-12-14 00:00:00Beregnung$ 		32016-12-13 00:00:00Ernte50kg' 	32016-12-13 00:00:00Er	l  ;�������p@�Z�d5                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           -		 =��rtzE:\temp\test1.ttp-scriptkgvj'		 9��gjE:\temp\aaa.ttp-scriptp(		 9��hvkE;	iSonstigesPräzisieren Sie  'Sonstiges':\nWelche Aktion?l b	mPflegemaßnahmePräzisieren Sie  'Pflagemaßnahme':\nWa#!	7MonitoringMonitoring: Ergebnis?.+	CPflegemaßnahmePflegemaßnahme: Was genau?8
)	YPflanzenschutzPflanzenschutz: Was? Wieviel? Wogegen?71	OHerbizidbehandlungHerbizidbehandlung: Was? Wieviel?#	;DüngungDüngung: Was? Wieviel?1-	GBodenbearbeitungBodenbearbeitung: Was genau? 	1AnzuchtAnzucht: Was?  Wo?% Versuchsende   	�PflanzenschutzPräzi Bonitur&	?SonstigesSonstiges: Welche Aktion?	 Pflanzung
 Hacken	 Ernte Drillsaat Beregnung� q ����������~ulcZQH?6%
��������������zqh_VMD;2) ��������������~ulcZQH?6-$	 ��������������ypg^ULC:1(.�����                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      w 		v 	t 1s 0r /q .p -o ,n *m (l 'k &j %i $h #g "f !e  d c b a ` _ ^ ] \ [ Z Y X W V U T S R Q P 
O 	N M L K J 0I /H .G -F ,E *D (C 'B &A %@ $? #> "= !<  ; : 9 8 7 6 5 4 3 2 1 0 / . - , + * ) ( ' 
& -% $ # " 	!    	    u 	 - #( #' 
- 
 
 
 
	 
 
 ( '   
 
	 	    	v 	 x 	w 	� 3p ���������������yrkd]VOHA:3,%�����������������xp                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        >	'I��neuer<'I��neuer VersuchE:\temp\dwd-stationsliste.htmlneu,	=��rtzE:\temp\test1.ttp-scriptkgvj&	9��gjE:\temp\aaa.ttp-scriptp'	9��hvkE:\temp\jki.ttp-scriptl&	9��v E:\temp\aaa.ttp-scripto(	9��,jhbE:\temp\aaa.ttp-scriptl'
	9��n ,E:�2f�1 ��0 ��0 ��/M�.#���(b� #� �-.�
,`�	*a�']�'\�'[�'Z�&�%�$"~!} |{zyxwvutsrqpo� �   k.j%i.h
2g
1f
0e
/d	 �c �ba`#_g   �    �����Q��D�{.��l-
�
�
�
D
	�	�	Z	$��j7��o$��D�~8��<��h�r � |        x�	 3)�# �2017-03-28 00:00:00PflanzenschutzRhododendron, Steinernema kraussei gegen Dickmaulrüssler. 2,0 l je Pflanzex�	 3)�# �2017-03-28 00:00:00PflanzenschutzRhododendron, Steinernema kraussei gegen Dickmaulrüssler. 2,0 l je Pflanzex�	 3)�# �2017-03-28 00:00:00PflanzenschutzRhododendron, Steinernema kraussei gegen Dickmaulrüssler. 2,0 l je Pflanzex�	 3)�# �2017-03-28 00:00:00PflanzenschutzRhododendron, Steinernema kraussei gegen Dickmaulrüssler. 2,0 l je Pflanze}�	 3)�- �2017-03-28 00:00:00PflanzenschutzRhododendron, Steinernema kraussei gegen Dickmaulrüssler. 2,0 - 2,5l je Pflanze�r�	 3�# �2017-03-28 00:00:00DüngungRhododendron
große Pflanzen: 120g Nitrophosca Perfekt, 80g Hornspäne, 3l Fetrilon 0,1%
mittlere Pflanzen: 90g N.P., 60g Hsp, 2l Fetrilon 0,1%
kleine Pflanzen: 70g N.P., 40g Hsp, 1,5l Fetrilon 0,1%
[� 3u �2017-04-12 00:00:00DrillsaatKamille `Bodelgold´, 4 Reihen, 10 g, Semdner Loch 2B� 3+92017-04-18 00:00:00PflegemaßnahmePelargonien entspitzen[�  3)m'2017-04-10 00:00:00PflanzenschutzFuchsien T 4,5,6 : Pirimor gegen Läuse, 300g/haW 3s2017-04-10 00:00:00AnzuchtDirektsaat in Matschtopfkisten (26 Stück)
C 4.3
D~ 3+=2017-03-29 00:00:00PflegemaßnahmeTopfen,12erPT,Substrat 5>} 3A2017-03-29 00:00:00AnzuchtTopfen, 12erPT, Substrat 5A| 3C-2017-04-07 00:00:00SonstigesAuswandern Bienen / HummelnA{ 3C-2017-04-04 00:00:00SonstigesEinwandern Bienen / HummelnLy 3],2017-03-31 00:00:00Anzuchtpikieren in 10 er Pt
Tonsubstrat
C 2.3Mx 3_,2017-03-23 00:00:00AnzuchtAussaat in Piki - Box
Tonsubstrat
C 3.1Aw 311(2017-03-24 00:00:00HerbizidbehandlungStomp Aqua 2,2l/haIv 3)I(2017-03-28 00:00:00Pflanzenschutzangießen   Spintor , Verimark2u 3-(2017-03-15 00:00:00BodenbearbeitungeggenCt 3-9 (2017-03-28 00:00:00Bodenbearbeitunganpflügen und fräsenMs 3_*2017-03-27 00:00:00Anzuchtpikieren in 10 er Pt
Tonsubstrat
C 2.3 1r 3'*2017-03-14 00:00:00AnzuchtAussaat C 3.1Cq 3-9#(2017-03-27 00:00:00Bodenbearbeitunganpflügen und fräsen4p 3-_(2017-03-23 00:00:00Bodenbearbeitungfräsen=o 3)1_(2017-03-23 00:00:00PflanzenschutzContans WG 4 kg/ha4n 3-^(2017-03-23 00:00:00Bodenbearbeitungfräsen=m 3)1^(2017-03-23 00:00:00PflanzenschutzContans WG 4 kg/ha4l 3-c(2017-03-23 00:00:00Bodenbearbeitungfräsen=k 3)1c(2017-03-23 00:00:00PflanzenschutzContanz WG 4 kg/ha4j 3-,(2017-03-23 00:00:00Bodenbearbeitungfräsen=i 3)1,(2017-03-23 00:00:00PflanzenschutzContans WG 4 kg/ha4h 3-*(2017-03-23 00:00:00Bodenbearbeitungfräsen<g 3)/*(2017-03-23 00:00:00PflanzenschutzContans WG 4kg/ha4f 3-'(2017-03-23 00:00:00Bodenbearbeitungfräsen=e 3)1'(2017-03-23 00:00:00PflanzenschutzContans WG 4 kg/ha4d 3-((2017-03-23 00:00:00Bodenbearbeitungfräsen<c 3)/((2017-03-23 00:00:00PflanzenschutzContans WG 4kg/haLb 3](2017-03-22 00:00:00Anzuchtpikieren in 10er PT 
Tonsubstrat
C 2.3Ka 3['2017-03-22 00:00:00Anzuchtpikieren in 10er PT
Tonsubstrat
C 2.3A_ 311$(2017-03-17 00:00:00HerbizidbehandlungStomp Aqua 2,5l/haA^ 311!(2017-03-17 00:00:00HerbizidbehandlungStomp Aqua 2,5l/haA] 311(2017-03-17 00:00:00HerbizidbehandlungStomp Aqua 2,5l/haM\ 3_'2017-03-14 00:00:00AnzuchtAussaat in Piki - Box
Tonsubstrat
C 3.1I[ 3W(2017-03-14 00:00:00AnzuchtAussaat C 3.1
Piki -Box
Tonsubstrat7Z 3/(2017-03-16 00:00:00DrillsaatWeizen: Untersaat8Y 33 (2017-03-16 00:00:00DüngungSpargel 100 kg N/ha8X 33#(2017-03-20 00:00:00DüngungSpargel 100 kg N/ha7W 3/$(2017-03-16 00:00:00DrillsaatZwiebeln 'Hector'>V 3?(2017-03-15 00:00:00DüngungKalkstickstoff 1000 kg/ha>U 3?(2017-03-15 00:00:00DüngungKalkstickstoff 1000 kg/ha&T 3!(2017-03-16 00:00:00Drillsaat&S 3(2017-03-16 00:00:00Drillsaat&R 3(2017-03-16 00:00:00DrillsaatBQ 3I2017-02-27 00:00:00AnzuchtEinzelkornau   �S   �    � V  ���1	��[��,%2?                                                                                                                                        � ��i�#��Wirksamkeit versch. Insektizide gegen Thrips an Brachyscome\\Bs-fs03\gf\12 - Schlagkart�: C�w�;��Kohlblattlaus mit Untersaat\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Böckmann\VP_Skizze Mehlige Kohlblattlaus_23_02.docxBekämpfung der mehligen Kohlblattlaus durch Untersaat/Banker
Plant System mit Getreideblattläusen für den Parasitoiden D. rapaeWeißkohl, Winterweizen  � ��}�#��Wirksamkeit versch. Insektizide gegen Thrips an Brachyscome\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Böckmann\VP_Skizze_Wirksamkeit Thrips Brachyscome.docxBiologische Wirksamkeit verschiedener Insektizide sowie
Wirkungssteigerung durch Zuckerzusatz 
zur Bekämpfung von Frankliniella occidentalis an Brachyscome 
im Gewächshaus.
- Ringversuch 2017 -
Brachyscome� k��+;��Reproduktionsrate von Diaretiella Rapae  Choice\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Böckmann\VP_Skizze_KS_Diaretiella_Choice_28_02_2017.docxReproduktionsrate von Diaretiella Rapae auf Rhopalosiphum padi und Mehliger KBLRosenkohl, WinterweizenX z W�[G��Kohlfliegenbekämpfung Rettich Satz 2\\Bs-fs03\gf\12 - Schlagkartei\2017_Versuchspläne\AG Hommes\Skizze�C
 W�cG��Kohlfliegenbekämpfung Rettich Satz 1\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Rettich_KF_2017_BS_Satz_1+2.docxBekämpfung Kleine KohlfliegeRettich� � W�qG��Kohlfliegenbekämpfung Rettich Satz 2\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\Skizze_VP_Rettich_KF_2017_BS_Satz_1+2.docxBekämpfung Kleine KohlfliegeRettichv � �C
 W�cG��Kohlfliegenbekämpfung Rettich Satz 2\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Rettich_KF_2017_BS_Satz_1+2.docxBekämpfung Kleine KohlfliegeRettichv  �R
 K�U{��Kohlfiegenbekämpfung Chinakohl\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Chinakohl_KF_2017 BS.docxBekämpfung Kleine Kohlfliege, Erdfloh u.a. SchädlingeChinakohl�n g�g���Bekämpfung Mottenschildlaus durch E. formosa\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Böckmann\VP_Skizze KMSL_EFormosa_23_02.docxBekämpfung der Kohlmottenschildlaus durch Encarsia formosaRosenkohl�o k��37��Applikationstechnik Thrips Brachyscome Geranien\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Böckmann\VP_Skizze_Applikationstechnik Thrips Brachyscome Geranie.docxPSM-Anlagerung bei Verwendung unterschiedlicher Düsentypen 
bei Breit- und Schmalblättrigen Zierpflanzen
- Applikationstechnik Versuch 2017 -
Brachyscome, Geranien�b
 I�q�#��Rostmilbe an Tomaten Frühjahr\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Böckmann\VP-Skizze Rostmilbe Frühjahr 2017.docxIst Rostmilbenbefall an Tomatenpflanzen mit spektrometrischen
Messungen unabhängig von der Wasserversorgung der Pflanzen 
detektierbar, bevor dieser mit dem bloßen Auge erkannt wird?
Tomate�"	 m��+;��Reproduktionsrate von Diaretiella Rapae NoChoice\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Böckmann\VP_Skizze_KS_Diaretiella_NoChoice_18_01_2017.docxReproduktionsrate von Diaretiella Rapae auf Rhopalosiphum padi und Mehliger KBLRosenkohl, Winterweizen�8
 Y�KG��Rettich Kohlfliegenbekämpfung  Satz 1\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Rettich_GH_2017.docxBekämpfung Kleine KohlfliegeRettich   � Q�9�)2��Schlupfversuch Porree-Minierfliege\\Bs-fs03\gf\12 - Schlagkartei\2017_Versuchspläne\AG Hommes\Versuch-2017-Schlupf.docxWie hängt die Schlupfrate�b Q�O�)2��Schlupfversuch Porree-Minierfliege\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\Versuch-2017-Schlupf.docxWie hängt die Schlupfrate der Porreeminierfliege mit der Bodentiefe zusammen?-	� ��		;s��>s�� �         �:
 S�=a��Prognose Möhrenfliege Ahlum Satz 4\\Bs-fs03\gf\12 - Schlagkartei\2017_Versuchspläne\AG Hommes\VP_Möhre_Prog_2017_Ah.docxPrognose und Befallserhebung MöhrenfliegeMöhre� � ]�]���Monitoringverfahren von Tomatenrostmilbe\\Bs-fs03\gf\12 - Schlagkartei\2017_Versuchspläne\AG Böckmann\VP_Skizze_Tomatenrostmilbe_C5_2017..docxUntersuchung der �{ A�g�E��Prognose Kohlfliege Satz 1\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Brokkoli_Prognose KF_2017 Hoe.docxPrognose Kleine Kohlfliege mittels Eimanschetten/
Bekämpfung von Kohldrehherzgallmücke 
Brokkoli	��H
 W�Ua��Prognose Möhrenfliege Hötzum Satz 3\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Möhre_Prog_2017_Hö.docxPrognose und Befallserhebung MöhrenfliegeMöhre�H
 W�Ua��Prognose Möhrenfliege Hötzum Satz 2\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Möhre_Prog_2017_Hö.docxPrognose und Befallserhebung MöhrenfliegeMöhre�H
 W�Ua��Prognose Möhrenfliege Hötzum Satz 1\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Möhre_Prog_2017_Hö.docxPrognose und Befallserhebung MöhrenfliegeMöhre�{ A�g�E��Prognose Kohlfliege Satz 6\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Brokkoli_Prognose KF_2017 Hoe.docxPrognose Kleine Kohlfliege mittels Eimanschetten/
Bekämpfung von Kohldrehherzgallmücke 
Brokkoli�{ A�g�E��Prognose Kohlfliege Satz 5\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Brokkoli_Prognose KF_2017 Hoe.docxPrognose Kleine Kohlfliege mittels Eimanschetten/
Bekämpfung von Kohldrehherzgallmücke 
Brokkoli�{ A�g�E��Prognose Kohlfliege Satz 4\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Brokkoli_Prognose KF_2017 Hoe.docxPrognose Kleine Kohlfliege mittels Eimanschetten/
Bekämpfung von Kohldrehherzgallmücke 
Brokkoli�{ A�g�E��Prognose Kohlfliege Satz 3\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Brokkoli_Prognose KF_2017 Hoe.docxPrognose Kleine Kohlfliege mittels Eimanschetten/
Bekämpfung von Kohldrehherzgallmücke 
Brokkoli�{ A�g�E��Prognose Kohlfliege Satz 2\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Brokkoli_Prognose KF_2017 Hoe.docxPrognose Kleine Kohlfliege mittels Eimanschetten/
Bekämpfung von Kohldrehherzgallmücke 
Brokkoli  � S�=a��Prognose Möhrenfliege Ahlum Satz 3\\Bs-fs03\gf\12 - Schlagkartei\2017_Versuchspläne\AG Hommes\VP_Möhre_Prog_2017_Ah.docxPrognose und Befallserhebung MöhrenfliegeMöhre  � S�=a��Prognose Möhrenfliege Ahlum Satz 2\\Bs-fs03\gf\12 - Schlagkartei\2017_Versuchspläne\AG Hommes\VP_Möhre_Prog_2017_Ah.docxPrognose und Befallserhebung MöhrenfliegeMöhre  � S�=a��Prognose Möhrenfliege Ahlum Satz 1\\Bs-fs03\gf\12 - Schlagkartei\2017_Versuchspläne\AG�E
 S�Sa��Prognose Möhrenfliege Ahlum Satz 4\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Möhre_Prog_2017_Ah.docxPrognose und Befallserhebung MöhrenfliegeMöhre�E
 S�Sa��Prognose Möhrenfliege Ahlum Satz 3\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Möhre_Prog_2017_Ah.docxPrognose und Befallserhebung MöhrenfliegeMöhre�E
 S�Sa��Prognose Möhrenfliege Ahlum Satz 2\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Möhre_Prog_2017_Ah.docxPrognose und Befallserhebung MöhrenfliegeMöhre�F
 S�Ua��Prognose Möhrenfliege Ahlum Satz 1\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Möhre_Prog_2017_Hö.docxPrognose und Befallserhebung MöhrenfliegeMöhre�y m�Q�/��Bekämpfung Möhrenfliege u. Möhrenminierfliege\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Möhre_Bek_2017_Ah.docxBekämpfung Möhrenfliege und Möhrenminierfliege mit Öko- PflanzenschutzmittelnMöhre    q 5k��
�rw���� q                                                                         �E1 �=��5��Wirksamkeit von Milch gegen Echten Mehltau (Erysiphe aquilegiae) an Delphinium nudicaule\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Idczak Götz\Versuchsplan Delphinium Echter Mehltau - Milch 2017.docxLässt sich Echter Mehltau an Rittersporn mit Milch kontrollieren?Delphinium nudicaule�a0 ���u)��Sortimentsprüfung Delphinium sp./Echter Mehltau (Erysiphe aquilegiae)\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Idczak Götz\Versuchsplan Delphinium-Sortiment Echter Mehltau 2017.docxGibt es Unterschiede in der Widerstandsfähigkeit verschiedener Delphiniumarten / -sorten gegenüber Echtem Mehltau?Delphinium sp.�./
 ?�[?*��Spinat Resistenzprüfung \\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\Smolka-KaKle\Falscher_Mehltau-Spinat.docxSpinat Resistenzprüfung Spinat�Q.
 O�O��Spargel SpargelfliegenbekämpfungD:\Lückenindikationen\Versuche\2017\VP_Spargel_2017.docxWirksamkeit verschiedener Insektizide und Applikationszeitpunkte auf die Spargelfliege an SpargelSpargel�- A��K7��Bienen Hummeln Brachyscome\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Böckmann\VP_Skizze_Bienen_Hummeln Brachyscome_03_04.docxKlärung möglicher effekte von neidrig konzentrierter Zuckerapplikation auf Bienen und HummelnBrachyscome multifida�7,
 )�Sq0��Gurken-Mehltau\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Wagner\VP_Gurken_C6-3_Mehltau.docxIrgendwas mit verschiedenen Mehltau-Arten an GurkeGurke�x* =�g�E0��Blütenendfäule Paprika\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Wagner\VP_Paprika_C6-4_Bluetenendfäule.docxKalziumdüngung als Gießbehandlung und Blattapplikation gegen 
Blütenendfäule an PaprikaPaprika�l( ��q�U#��Tomatenrostmilbe: Untersuchung  von Bekämpfungsmöglichkeiten \\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Böckmann\VP_Skizze_Tomatenrostmilbe_C7_2017.docxUntersuchung der Effizienz von verschiedenen chemischen,
biologischen und mechanischen Bekämpfungsmöglichkeiten 
gegen die Tomatenrostmilbe Aculops lycopersici.Tomate�*' W�q�#��Tomatenrostmilbe: Monitoringverfahren\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Böckmann\VP_Skizze_Tomatenrostmilbe_C5_2017.docxUntersuchung der Effizienz von Monitoringverfahren zur frühen 
Erkennung von Aculops lycopersici Befall. (Tomatenrostmilbe)Tomate�& w�S�;��Bekämpfung  Thrips Minierfliegen an Porree / Hötzum\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Porree_Bek_2017_Hö.docxKontrolle von Thripsen und Lauchminierfliegen mit konventionellen PflanzenschutzmittelnPorree�w% s�Q�%��Bekämpfung  Thrips Minierfliegen an Porree / Ahlum\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Porree_Bek_2017_Ah.docxKontrolle von Thripsen und Lauchminierfliegen mit Öko-PflanzenschutzmittelnPorree�F$
 M�[a��Prognose Möhrenfliege BS Satz 4\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Möhre_Prognose_2017_BS.docxPrognose und Befallserhebung MöhrenfliegeMöhre�F#
 M�[a��Prognose Möhrenfliege BS Satz 3\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Möhre_Prognose_2017_BS.docxPrognose und Befallserhebung MöhrenfliegeMöhre�F"
 M�[a��Prognose Möhrenfliege BS Satz 2\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Möhre_Prognose_2017_BS.docxPrognose und Befallserhebung MöhrenfliegeMöhre�G!
 M�[c��Prognose Möhrenfliege BS Satz 1\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Möhre_Prognose_2017_BS.docx Prognose und Befallserhebung MöhrenfliegeMöhre�H 
 W�Ua��Prognose Möhrenfliege Hötzum Satz 4\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG Hommes\VP_Möhre_Prog_2017_Hö.docxPrognose und Befallserhebung MöhrenfliegeMöhrex A � �����|fP:#����lQ6����}b�uC�� �4
�
�
h
$	�	�	X	��E �v1��O��G�|	�#�=�rr                                                    Z. �!GH21 B4-2GTemperaturregelung
Assimilationslicht
Rinnentische 70 m²
InsektendichtT- �GH21 B4-1GTemperaturregelung
Assimilationslicht
Tische 84 m²
InsektendichtVM �GH20 C3-1GTemperaturregelung mit Solekühlung
Assimilationslicht
Tische 12 m²qL �OGH20 C2-3GTemperaturregelung mit adiabatischer Kühlung
Luftbefeuchtung
Assimilationslicht
Tische 47 m²qK �OGH20 C2-2GTemperaturregelung mit adiabatischer Kühlung
Luftbefeuchtung
Assimilationslicht
Tische 24 m²qJ �OGH20 C2-1GTemperaturregelung mit adiabatischer Kühlung
Luftbefeuchtung
Assimilationslicht
Tische 24 m²qI �OGH20 C1-4GTemperaturregelung mit adiabatischer Kühlung
Luftbefeuchtung
Assimilationslicht
Tische 24 m²qH �OGH20 C1-3GTemperaturregelung mit adiabatischer Kühlung
Luftbefeuchtung
Assimilationslicht
Tische 24 m²qG �OGH20 C1-2GTemperaturregelung mit adiabatischer Kühlung
Luftbefeuchtung
Assimilationslicht
Tische 24 m²qF �OGH20 C1-1GTemperaturregelung mit adiabatischer Kühlung
Luftbefeuchtung
Assimilationslicht
Tische 24 m²VE �GH21 B6-5GTemperaturregelung mit Solekühlung
Assimilationslicht
Tische 14 m²VD �GH21 B6-4GTemperaturregelung mit Solekühlung
Assimilationslicht
Tische 14 m²VC �GH21 B6-3GTemperaturregelung mit Solekühlung
Assimilationslicht
Tische 14 m²VB �GH21 B6-2GTemperaturregelung mit Solekühlung
Assimilationslicht
Tische 14 m²VA �GH21 B6-1GTemperaturregelung mit Solekühlung
Assimilationslicht
Tische 14 m²C@ !sGH21 B5-18GTemperaturregelung
Assimilationslicht
Tisch 5 m²C? !sGH21 B5-17GTemperaturregelung
Assimilationslicht
Tisch 5 m²C> !sGH21 B5-16GTemperaturregelung
Assimilationslicht
Tisch 5 m²C= !sGH21 B5-15GTemperaturregelung
Assimilationslicht
Tisch 5 m²C< !sGH21 B5-14GTemperaturregelung
Assimilationslicht
Tisch 5 m²C; !sGH21 B5-13GTemperaturregelung
Assimilationslicht
Tisch 5 m²C: !sGH21 B5-12GTemperaturregelung
Assimilationslicht
Tisch 5 m²C9 !sGH21 B5-11GTemperaturregelung
Assimilationslicht
Tisch 5 m²C8 !sGH21 B5-10GTemperaturregelung
Assimilationslicht
Tisch 5 m²B7 sGH21 B5-9GTemperaturregelung
Assimilationslicht
Tisch 5 m²B6 sGH21 B5-8GTemperaturregelung
Assimilationslicht
Tisch 5 m²B5 sGH21 B5-7GTemperaturregelung
Assimilationslicht
Tisch 5 m²B4 sGH21 B5-6GTemperaturregelung
Assimilationslicht
Tisch 5 m²B3 sGH21 B5-5GTemperaturregelung
Assimilationslicht
Tisch 5 m²B2 sGH21 B5-4GTemperaturregelung
Assimilationslicht
Tisch 5 m²B1 sGH21 B5-3GTemperaturregelung
Assimilationslicht
Tisch 5 m²B0 sGH21 B5-2GTemperaturregelung
Assimilationslicht
Tisch 5 m²B/ sGH21 B5-1GTemperaturregelung
Assimilationslicht
Tisch 5 m²G M�GH21 B4-2GTemperaturregelung
Assimilationslicht
Rinnentische 70 m²6, [GH21 B3-2GTemperaturregelung
Rinnentische 84 m²D+ wGH21 B3-1GTemperaturregelung
Assimilationslicht
Tische 84 m²0* OGH21 B2-2GTemperaturregelung
Tische 60 m²D) wGH21 B2-1GTemperaturregelung
Assimilationslicht
Tische 60 m²D( wGH21 B1-2GTemperaturregelung
Assimilationslicht
Tische 60 m²D' wGH21 B1-1GTemperaturregelung
Assimilationslicht
Tische 60 m²   'BS 9.4Aca. 24 x 45 m% 'BS 9.3Aca. 24 x 45 m$ 'BS 9.2Aca. 24 x 45 m# 'BS 9.1Aca. 24 x 45 m" %BS 8.4Aca 35 x 50 m! %BS 8.3Aca 35 x 50 m  %BS 8.2Aca 35 x 50 m %BS 8.1Aca 35 x 50 m 'BS 7.4Aca. 20 x 40 m 'BS 7.3Aca. 20 x 40 m 'BS 7.2Aca. 20 x 40 m 'BS 7.1Aca. 20 x 40 m# 9Ahlum 3A23 x 30 m
Ökofläche# 9Ahlum 2A23 x 30 m
Ökofläche# 9Ahlum 1A23 x 30 m
Ökofläche Hö 11A13 x 40 m Hö 0A12 x 40 m Hö 10A27 x 40 m Hö 9A27 x 40 m Hö 8A27 x 40 m Hö 7A27 x 40 m Hö 6A27 x 40 m Hö 5A27 x 40 m Hö 4A27 x 40 m Hö 3A27 x 40 m Hö 2A27 x 40 m Hö 1A27 x 40 m( % �����O��<�R
�t6 �W�~@,����j�U��b�(�Je6ZL��O
�
k
 	�	y	.��r�               uY �WGH20 C4-4GTemperaturregelung mit adiabatischer Kühlung
Luftbefeuchtung
Assimilationslicht
Betonboden 35 m²<\ gGH20 C5-3GTemperaturregelung 
gewachsener Boden 35 m²<[ gGH20 C5-2GTemperaturregelung 
gewachsener Boden 35 m²<Z gGH20 C5-1GTemperaturregelung 
gewachsener Boden 35 m²;c iGH20 C8GTemperaturregelung 
gewachsener Boden 160 m²;b iGH20 C7GTemperaturregelung 
gewachsener Boden 160 m²<a gGH20 C6-4GTemperaturregelung 
gewachsener Boden 35 m²]f �'GH20 K112KTemperaturregelung, Feuchteregelung
Stellfläche: Tische
Assimilationslicht]e �'GH20 K111KTemperaturregelung, Feuchteregelung
Stellfläche: Tische
Assimilationslicht� Ahlum 4A23 x 30 m<` gGH20 C6-3GTemperaturregelung 
gewachsener Boden 35 m²<_ gGH20 C6-2GTemperaturregelung 
gewachsener Boden 35 m²<^ gGH20 C6-1GTemperaturregelung 
gewachsener Boden 35 m²� �CA K144aKnurVN �GH20 C3-2GTemperaturregelung mit Solekühlung
Assimilationslicht
Tische 12 m²]q �'GH21 K108KTemperaturregelung, Feuchteregelung
Stellfläche: Tische
Assimilationslichtr 	 �s#d 9Ahlum 4A23 x 30 m
ÖkoflächeVO �GH20 C3-3GTemperaturregelung mit Solekühlung
Assimilationslicht
Tische 12 m²mp �GGH21 K010KTemperaturregelung - auch für niedrige Temperaturen
Stellfläche: Tisch
Assimilationslichtmo �GGH21 K009KTemperaturregelung - auch für niedrige Temperaturen
Stellfläche: Tisch
Assimilationslicht8n _GH20 K123KLagerraum
Temperaturregelung
kein Licht8l _GH20 K121KLagerraum
Temperaturregelung
kein LichtrVP �GH20 C3-4GTemperaturregelung mit Solekühlung
Assimilationslicht
Tische 12 m²]j �'GH20 K116KTemperaturregelung, Feuchteregelung
Stellfläche: Tische
Assimilationslicht]i �'GH20 K115KTemperaturregelung, Feuchteregelung
Stellfläche: Tische
Assimilationslicht]h �'GH20 K114KTemperaturregelung, Feuchteregelung
Stellfläche: Tische
Assimilationslicht]g �'GH20 K113KTemperaturregelung, Feuchteregelung
Stellfläche: Tische
Assimilationslicht�Y �C K156KInsektenzuchten Forst
Temperaturregelung
Leuchtstoffröhren!� 7C K061KLagerraum
kein LichtH�
 �C K060KTemperaturregelung
Stellfläche: Tisch
AssimilationslichtH�	 �C K059KTemperaturregelung
Stellfläche: Tisch
AssimilationslichtY� �%C K057KQuarantäneraum
Temperaturregelung
Stellfläche: Tisch
AssimilationslichtH� �C K050KTemperaturregelung
Stellfläche: Tisch
AssimilationslichtH� �C K049KTemperaturregelung
Stellfläche: Tisch
AssimilationslichtH� �C K048KTemperaturregelung
Stellfläche: Tisch
AssimilationslichtH� �C K047KTempe�Y �uGH20 C4-4GTemperaturregelung mit adiabatischer Kühlung
Luftbefeuchtung
Assimilationslicht
Betonboden 35 m²
Insektendicht�X �uGH20 C4-3GTemperaturregelung mit adiabatischer Kühlung
Luftbefeuchtung
Assimilationslicht
Betonboden 35 m²
InsektendichtaW �/GH20 C4-2GTemperaturregelung mit adiabatischer Kühlung
Luftbefeuchtung
Betonboden 35 m²aV �/GH20 C4-1GTemperaturregelung mit adiabatischer Kühlung
Luftbefeuchtung
Betonboden 35 m²VU �GH20 C3-9GTemperaturregelung mit Solekühlung
Assimilationslicht
Tische 12 m²VT �GH20 C3-8GTemperaturregelung mit Solekühlung
Assimilationslicht
Tische 12 m²VS �GH20 C3-7GTemperaturregelung mit Solekühlung
Assimilationslicht
Tische 12 m²VR �GH20 C3-6GTemperaturregelung mit Solekühlung
Assimilationslicht
Tische 12 m²VQ �GH20 C3-5GTemperaturregelung mit Solekühlung
Assimilationslicht
Tische 12 m²<] gGH20 C5-4GTemperaturregelung 
gewachsener Boden 35 m²   �A K032KInsektenzuchtraum
Temperaturregelung
LeuchtstoffröhrenEt A K031KInsektenzuchtraum
Temperaturregelung
Leuchtstoffröhren   c  GH20 C8G]r �'GH21 K109KTemperaturregelung, Feuchteregelung
Stellfläche: Tische
Assimilationslicht8m _GH20 K122KLagerraum
Temperaturregelung
kein Licht/k MGH20 K117KSaatgutlager
TemperaturregelungQ 6} z3��>���u���;��M
�
l
!	�	z	/	�d@(������s_K7"������}4���                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         @�# =QGH20 FreilandtischanlageS10 Betontische mit jeweils 7.1 m²�( 5 GH21 Zwischen 4 u. 6S�' 5 GH21 Zwischen 3 u. 4S�& 5 GH21 Zwischen 2 u. 3S�% 5 GH21 Zwischen 1 u. 2S�$ 5 GH21 Zwischen 5 u. 1S   > 3QFreilandtischanlageS10 Betontische mit jeweils 7.1 m²�" %BS Kasten 17S�! %BS Kasten 16S�  %BS Kasten 15S� %BS Kasten 14S� %BS Kasten 13S� %BS Kasten 12S� %BS Kasten 11S� %BS Kasten 10S� #BS Kasten 9S� #BS Kasten 8S� #BS Kasten 7S� #BS Kasten 6S� #BS Kasten 5S� #BS Kasten 4S� #BS Kasten 3S� #BS Kasten 2S� #BS Kasten 1S"� -%BS AuffangbeckenSRhodofläche� + BS Bionethaus 2S� + BS Bionethaus 1S!� 7BS 9.5SGehölze
Rhodo-HeckeW� �GH20 K118KTemperaturregelung - auch unter 0 °C
Stellfläche: Regale
kein LichtJ� �C K156KInsektenzuchten Forst
Temperaturregelung
Leuchtstoffröhren!� 7C K061KLagerraum
kein LichtH�
 �C K060KTemperaturregelung
Stellfläche: Tisch
AssimilationslichtH�	 �C K059KTemperaturregelung
Stellfläche: Tisch
AssimilationslichtY� �%C K057KQuarantäneraum
Temperaturregelung
Stellfläche: Tisch
AssimilationslichtH� �C K050KTemperaturregelung
Stellfläche: Tisch
AssimilationslichtH� �C K049KTemperaturregelung
Stellfläche: Tisch
AssimilationslichtH� �C K048KTemperaturregelung
Stellfläche: Tisch
AssimilationslichtH� �C K047KTemperaturregelung
Stellfläche: Tisch
AssimilationslichtK� �	B K311KTageslicht-Zuchtraum
Temperaturregelung
(Assimilationslicht)@� uB K302KTemperaturregelung
Stellfläche: Regale
kein LichtZ� �'B K301KTemperaturregelung, Feuchteregelung
Stellfläche: Regale
LeuchtstoffröhrenZ�  �'B K202KTemperaturregelung, Feuchteregelung
Stellfläche: Regale
Leuchtstoffröhren  B K201K8~ eB K102KPilzkulturen
Temperaturregelung
kein Licht8} eB K101KPilzkulturen
Temperaturregelung
kein LichtY| �%B K112KTemperaturregelung, Feuchteregelung
Stellfläche: Tisch
Assimilationslicht{ -B K003KThermostatenraumz -B K001KThermostatenraumIy �A K168KTemperaturregelung
Stellfläche: Regale
Leuchtstoffröhren@x uA K167KTemperaturregelung
Stellfläche: Regale
ohne LichtAw uA K144bKTemperaturregelung
Stellfläche: Regale
ohne Lichtiv �CA K144aKnur für niedrige Temperaturen ( Kühlung, keine Heizung)
Stellfläche: Regale
ohne LichtEu A K032KInsektenzuchtraum
Temperaturregelung
LeuchtstoffröhrenEt A K031KInsektenzuchtraum
Temperaturregelung
Leuchtstoffröhren�s �sGH21 K110KTemperaturregelung - auch für niedrige Temperaturen
Stellfläche: Tisch
Assimilationslicht
kein Wasseranschluss � . � ��lD��Q��D�{.��l-
�
�
�
D
	�	�	Z	$��j7��o$��D�d ��h�c�m � �                                          T~ 3+]2017-03-29 00:00:00PflegemaßnahmeBrachyscome: Topfen, 12er PT, Substrat 5  � 3)�# �2017-03-28 00:00:00PflanzenschutzRhododendron, Steinernema kraussei gegen Dickmaulrüssler. 2,0 l je Pflanzex�	 3)�# �2017-03-28 00:00:00PflanzenschutzRhododendron, Steinernema kraussei gegen Dickmaulrüssler. 2,0 l je Pflanzex�	 3)�# �2017-03-28 00:00:00PflanzenschutzRhododendron, Steinernema kraussei gegen Dickmaulrüssler. 2,0 l je Pflanzex�	 3)�# �2017-03-28 00:00:00PflanzenschutzRhododendron, Steinernema kraussei gegen Dickmaulrüssler. 2,0 l je Pflanze}�	 3)�- �2017-03-28 00:00:00PflanzenschutzRhododendron, Steinernema kraussei gegen Dickmaulrüssler. 2,0 - 2,5l je Pflanze  O	 3�K �2017-03-28 00:00:00DüngungRhododendron
große Pflanzen: 120g Novatec classik, 80g Hornspäne, 3l Fetrilon 0,1%
mittlere Pflanzen: 90g Novatec classik., 60g Hsp, 2l Fetrilon 0,1%
kleine Pflanzen: 70g Novatec classik., 40g Hsp, 1,5l Fetrilon 0,1%
B� 3+92017-04-18 00:00:00PflegemaßnahmePelargonien entspitzen[�  3)m'2017-04-10 00:00:00PflanzenschutzFuchsien T 4,5,6 : Pirimor gegen Läuse, 300g/haW 3s2017-04-10 00:00:00AnzuchtDirektsaat in Matschtopfkisten (26 Stück)
C 4.3
X} 3u2017-03-29 00:00:00AnzuchtBrachyscome+Pelargonien: Topfen, 12er PT, Substrat 5A| 3C-2017-04-07 00:00:00SonstigesAuswandern Bienen / HummelnA{ 3C-2017-04-04 00:00:00SonstigesEinwandern Bienen / HummelnLy 3],2017-03-31 00:00:00Anzuchtpikieren in 10 er Pt
Tonsubstrat
C 2.3Mx 3_,2017-03-23 00:00:00AnzuchtAussaat in Piki - Box
Tonsubstrat
C 3.1Aw 311(2017-03-24 00:00:00HerbizidbehandlungStomp Aqua 2,2l/haIv 3)I(2017-03-28 00:00:00Pflanzenschutzangießen   Spintor , Verimark2u 3-(2017-03-15 00:00:00BodenbearbeitungeggenCt 3-9 (2017-03-28 00:00:00Bodenbearbeitunganpflügen und fräsenMs 3_*2017-03-27 00:00:00Anzuchtpikieren in 10 er Pt
Tonsubstrat
C 2.3 1r 3'*2017-03-14 00:00:00AnzuchtAussaat C 3.1Cq 3-9#(2017-03-27 00:00:00Bodenbearbeitunganpflügen und fräsen4p 3-_(2017-03-23 00:00:00Bodenbearbeitungfräsen=o 3)1_(2017-03-23 00:00:00PflanzenschutzContans WG 4 kg/ha4n 3-^(2017-03-23 00:00:00Bodenbearbeitungfräsen=m 3)1^(2017-03-23 00:00:00PflanzenschutzContans WG 4 kg/ha4l 3-c(2017-03-23 00:00:00Bodenbearbeitungfräsen=k 3)1c(2017-03-23 00:00:00PflanzenschutzContanz WG 4 kg/ha4j 3-,(2017-03-23 00:00:00Bodenbearbeitungfräsen=i 3)1,(2017-03-23 00:00:00PflanzenschutzContans WG 4 kg/ha4h 3-*(2017-03-23 00:00:00Bodenbearbeitungfräsen<g 3)/*(2017-03-23 00:00:00PflanzenschutzContans WG 4kg/ha4f 3-'(2017-03-23 00:00:00Bodenbearbeitungfräsen=e 3)1'(2017-03-23 00:00:00PflanzenschutzContans WG 4 kg/ha4d 3-((2017-03-23 00:00:00Bodenbearbeitungfräsen<c 3)/((2017-03-23 00:00:00PflanzenschutzContans WG 4kg/haLb 3](2017-03-22 00:00:00Anzuchtpikieren in 10er PT 
Tonsubstrat
C 2.3Ka 3['2017-03-22 00:00:00Anzuchtpikieren in 10er PT
Tonsubstrat
C 2.3A_ 311$(2017-03-17 00:00:00HerbizidbehandlungStomp Aqua 2,5l/haA^ 311!(2017-03-17 00:00:00HerbizidbehandlungStomp Aqua 2,5l/haA] 311(2017-03-17 00:00:00HerbizidbehandlungStomp Aqua 2,5l/haM\ 3_'2017-03-14 00:00:00AnzuchtAussaat in Piki - Box
Tonsubstrat
C 3.1I[ 3W(2017-03-14 00:00:00AnzuchtAussaat C 3.1
Piki -Box
Tonsubstrat7Z 3/(2017-03-16 00:00:00DrillsaatWeizen: Untersaat8Y 33 (2017-03-16 00:00:00DüngungSpargel 100 kg N/ha8X 33#(2017-03-20 00:00:00DüngungSpargel 100 kg N/ha7W 3/$(2017-03-16 00:00:00DrillsaatZwiebeln 'Hector'>V 3?(2017-03-15 00:00:00DüngungKalkstickstoff 1000 kg/ha>U 3?(2017-03-15 00:00:00DüngungKalkstickstoff 1000 kg/ha&T 3!(2017-03-16 00:00:00Drillsaat&S 3(2017-03-16 00:00:00Drillsaat&R 3(2017-03-16 00:00:00DrillsaatBQ 3I2017-02-27 00:00:00AnzuchtEinzelkornaussaat
6 Mtk
C4.3{ � ���%�/�'	��y�J���f
�
{	c�O�y5��L��r=��f"���X�                                                                                    K�" 3Y,2017-04-18 00:00:00Düngung50g/m² Nitrophoska spezial (12/12/17)I�8 3S �2017-04-24 00:00:00DüngungKamille, Novatec classik, 50g / m²0�7 3!c(2017-04-24 00:00:00DrillsaatOelrettich0�6 3!^(2017-04-24 00:00:00DrillsaatOelrettich0�5 3!_(2017-04-24 00:00:00DrillsaatOelrettich.�?�  3A(2017-04-18 00:00:00Düngung50g/m² Nitophoska spezialA� 3C-2017-04-07 00:00:00SonstigesAbspülen der Zuckerlösung��	 3�{ �2017-03-27 00:00:00DüngungRhododendron im und neben Bionet,
Novatec classik: 80g/Pflanze
Hornspäne: 50g/Pflanze
Fetrilon: 0,1 %, 2l / Pflanze��	 3�K �2017-03-27 00:00:00DüngungRhododendron, 
Novatec classik: 40g/m²
Schwefelsaures Ammoniak: 40g/m²
Hornspäne: 50g/m²��	 3�K �2017-03-27 00:00:00DüngungRhododendron, 
Novatec classik: 40g/m²
Schwefelsaures Ammoniak: 40g/m²
Hornspäne: 50g/m²x�	 3)�# �2017-03-28 00:00:00PflanzenschutzRhododendron, Steinernema kraussei gegen Dickmaulrüssler. 2,0 l je Pflanzex�	 3)�# �2017-03-28 00:00:00PflanzenschutzRhododendron, Steinernema kraussei gegen Dickmaulrüssler. 2,0 l je Pflanzex�	 3)�# �2017-03-28 00:00:00PflanzenschutzRhododendron, Steinernema kraussei gegen Dickmaulrüssler. 2,0 l je Pflanzex�	 3)�# �2017-03-28 00:00:00PflanzenschutzRhododendron, Steinernema kraussei gegen Dickmaulrüssler. 2,0 l je Pflanze}�	 3)�- �2017-03-28 00:00:00PflanzenschutzRhododendron, Steinernema kraussei gegen Dickmaulrüssler. 2,0 - 2,5l je Pflanze�0�	 3� �2017-03-28 00:00:00DüngungRhododendron
große Pflanzen:    120g Novatec classik
                                  80g Hornspäne
mittlere Pflanzen: 90g Novatec classik
                                 60g Hornspäne
kleine Pflanzen:    70g Novatec classik
                                 40g Hornspäne
Nur Sorte `Cunningham´s White´:
große Pflanzen :   3l Fetrilon 0,1%
mittlere Pflanzen: 2l Fetrilon 0,1%��	 3�K �2017-03-27 00:00:00DüngungRhododendron, 
Novatec classik: 40g/m²
Schwefelsaures Ammoniak: 40g/m²
Hornspäne: 50g/m²��	 3�K �2017-03-27 00:00:00DüngungRhododendron, 
Novatec classik: 40g/m²
Schwefelsaures Ammoniak: 40g/m²
Hornspäne: 50g/m²��	 3�K �2017-03-27 00:00:00DüngungRhododendron, 
Novatec classik: 40g/m²
Schwefelsaures Ammoniak: 40g/m²
Hornspäne: 50g/m²�
�	 3�S �2017-03-30 00:00:00DüngungRhododendron-Hecke, Novatec classik, 100g / m². Gehölze ( Pieris etc. Novatec classik: 50 g / m²J� 3U �2017-03-27 00:00:00DüngungEchinops, Novatec classik: 50 g/ m²L� 3Y �2017-03-27 00:00:00DüngungDelphinium, Novatec classik: 50g / m²L� 3Y �2017-03-27 00:00:00DüngungDelphinium, Novatec classik: 50 g /m²V� 3m �2017-03-27 00:00:00DüngungGehölze+Delphinium, Novatec classic: 50 g / m²Y� 3s �2017-03-27 00:00:00DüngungKamille (Herbst-Aussaat),  Novatec classik: 50g/m²]� 3{ �2017-03-27 00:00:00DüngungBuxus, Hüttenkalk: 100g/m² + Novatec classik: 75g/m²�  ]� 3{ �2017-03-27 00:00:00DüngungBuxus, Hüttenkalk: 100g/m² + Novatec classik: 75g/m²E  ]� 3{ �2017-03-27 00:00:00DüngungBuxus, Hüttenkalk: 100g/m² + Novatec classik: 75g/m²�  ]� 3{ �2017-03-27 00:00:00DüngungBuxus, Hüttenkalk: 100g/m² + Novatec classik: 75g/m²�  ]� 3{ �2017-03-27 00:00:00DüngungBuxus, Hüttenkalk: 100g/m² + Novatec classik: 75g/m²g�	 3�
2017-04-20 00:00:00SonstigesWinterweizen beimpft mit R.padi aus GH B 5.2.
Pro WDH 10000 BL.&� 3 '#2017-04-19 00:00:00Pflanzung&� 3(#2017-04-19 00:00:00Pflanzung   U� 3k �2017-B� 3+92017-04-18 00:00:00PflegemaßnahmePelargonien entspitzen[�
 3u �2017-04-12 00:00:00DrillsaatKamille `Bodelgold´, 10 g, 4 Reihen, Semdner Loch 2x�		 3)�# �2017-03-28 00:00:00PflanzenschutzRhododendron, Steinernema kraussei gegen Dickmaulrüssler. 2,0 l je Pflanze} 7 � v(���R��d.
�
�
�
e
!	�	�	y	H	��c4���M$��^) �o'�n�;�f] � � �;�?��                   T�S 3m-
2017-05-04 00:00:00BoniturBonitur: 3 x Flugbeobachtung (11:00/13:30/15:30)��R	 3�G-
2017-05-03 00:00:00BoniturNachbonitur: Flugbeobachtung direkt nach Spritzung (13:30) und 3 std. nach Spritzung (16:30).�Q�M	 3�e-
2017-04-06 00:00:00BoniturNachbonitur: Flugbeobachtung direkt nach Spritzung. 
Nachbonitur: Flugbeobachtung 4 std. nach Spritzung.
(Bei den Flugbonituren  wurden keine Auffälligkeiten beobachtet)?�K 3C-
2017-04-06 00:00:00BoniturVorbonitur: Flugbeobachtung� 	 3�y-&�Z 3 *2017-05-08 00:00:00PflanzungI�Q 3S-(2017-05-03 00:00:00SonstigesSpritzung der Zuckerlösung (13:00)Z�P 3y-
2017-05-03 00:00:00BoniturVorbonitur vor Appl.: 2 x Flugbeobachtung (7:30/12:00)T�O 3m-
2017-05-02 00:00:00BoniturBonitur: 3 x Flugbeobachtung (10:20/12:45/15:00)A�L 3C-
2017-04-06 00:00:00SonstigesSpritzung der Zuckerlösungs�I	 3)�(2017-05-03 00:00:00PflanzenschutzWinterweizen hat Gelbrost, wurde gespritzt mit Opus Top 1l/ 600 Wasser.l�H	 3�
2017-05-02 00:00:00SonstigesWinterweizen beimpft mit R.padi aus GH B 5.2 / pro WDH ca 17700 BL.
G�G 3S&2017-03-06 00:00:00AnzuchtAussaat in Piki -Box . je 500 Korn E�F 3O2017-05-02 00:00:00AnzuchtAussaat, Einzelkorn direkt in MtkE�E 3O2017-03-27 00:00:00AnzuchtAussaat, Einzelkorn direkt in MtkF�D 3Q2017-03-27 00:00:00AnzuchtAussaat , Einzelkorn direkt in Mtk&�C 3 (2017-05-03 00:00:00Pflanzung2�B 3-(2017-05-03 00:00:00BodenbearbeitungeggenA�A 3C-2017-05-02 00:00:00SonstigesEinwandern Bienen / Hummeln=�@ 3=(2017-05-02 00:00:00DüngungKalkstickstoff 1000kg/ha?�? 31-(2017-05-02 00:00:00HerbizidbehandlungStomp Aqua 3L/ha&�> 3 (2017-05-02 00:00:00Pflanzung2�= 3-(2017-04-20 00:00:00Bodenbearbeitungeggen?�< 31-(2017-03-30 00:00:00HerbizidbehandlungStomp Aqua 3L/ha&�; 3 (2017-03-30 00:00:00PflanzungD�: 3)?(2017-04-28 00:00:00Pflanzenschutzangießen Spintor,Vrimark,�9 3/2017-04-24 00:00:00AnzuchtAussaat I�8 3S �2017-04-25 00:00:00DüngungKamille, Novatec classik, 50g / m²0�7 3!c(2017-04-24 00:00:00DrillsaatOelrettich0�6 3!^(2017-04-24 00:00:00DrillsaatOelrettich0�5 3!_(2017-04-24 00:00:00DrillsaatOelrettich.�4 3d(2017-04-24 00:00:00DrillsaatKleegrasA�3 311(2017-04-24 00:00:00HerbizidbehandlungStomp Aqua 2.5l/ha,�2 3(2017-04-24 00:00:00DrillsaatAlmaro2�1 3-(2017-04-20 00:00:00BodenbearbeitungeggenA�0 311"(2017-04-24 00:00:00HerbizidbehandlungStomp Aqua 2.5l/ha,�/ 3"(2017-04-24 00:00:00DrillsaatAlmaro2�. 3-"(2017-04-20 00:00:00Bodenbearbeitungeggen-�- 3(2017-04-24 00:00:00DrillsaatNegovia2�, 3-(2017-04-20 00:00:00Bodenbearbeitungeggen3�+ 3'(2017-04-24 00:00:00SonstigesUmrandung VG5<�* 31'(2017-04-21 00:00:00HerbizidbehandlungButisan 1l/ha7�) 3/(2017-04-20 00:00:00DrillsaatRettich Sorte Rex2�( 3-(2017-04-20 00:00:00Bodenbearbeitungeggen=�' 3;'2017-04-18 00:00:00SonstigesBändchengewebe verlegt4�& 3-'(2017-04-18 00:00:00Bodenbearbeitungfräsen<�% 39(2017-04-18 00:00:00SonstigesBändchengewebeverlegt4�$ 3-(2017-04-18 00:00:00Bodenbearbeitungfräsen&�# 3 ,2017-04-20 00:00:00PflanzungK�" 3Y,2017-04-18 00:00:00Düngung50g/m² Nitrophoska spezial (12/12/17)K�! 3Y'2017-04-18 00:00:00Düngung50g/m² Nitrophoska spezial (12/12/17)   $ 3A(2017-04-18 00:00:00DünT�Y 3i2'2017-05-03 00:00:00SonstigesKlimakammer auf 25°C, 40% RH und 14:10 h D:N,d�X	 3�2'2017-05-04 00:00:00SonstigesWeitere Blätter aus Zucht und alter Rhodohecke dazu gestellt��W	 3�a2'2017-05-02 00:00:00SonstigesRhodo-Blattmaterial aus Gelände kath. Friedhof (n=10 Sträucher) gesammelt und in Minicontainer gestellt.A�V 3C-2017-05-05 00:00:00SonstigesAbspülen der Zuckerlösung~�T	 3�;-
2017-05-05 00:00:00SonstigesAbwanderung der Bienen (Hummeln bleiben noch einen Tag länger zur Beobachtung stehen).   � �                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        �2 c�O�K-'��Andromedanetzwanzebekämpfung mit Nematoden\\Bs-fs03\gf\11_dabaschlag\Versuchsplaene\2017_Versuchspläne\AG urbanes Grün\ANW1_LVNEMA_2.docxWie wirksam ist eine Blattapplikation von nematoden gegen Larvenstadien der Andromedanetzwanze?Rhododendron sp.        ��H��^#��q2���H��m6
�
�
g
-	�	�	�	O	��g,���J��^��X!                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           5/G	3WilkeAndreas4435andreas.wilke@julius-kuehn.deG.-Y	2Rempe-VespermannNelli4451nelli.rempe-vespermann@julius-kuehn.de7-I	1WescheJohanna4405johanna.wesche@julius-kuehn.de<,G	0Wagner_sStefan4411/4449stefan.wagner@julius-kuehn.deD+Q	/TrautmannReinhold4616/4617reinhold.trautmann@julius-kuehn.de;*M	.TrautmannDagmar4608dagmar.trautmann@julius-kuehn.de7)I	-ThieleHenning4616henning.thiele@julius-kuehn.de9(!K	,StreithoffElke4653elke.streithoff@julius-kuehn.de9'K	+StraussKirsten4446kirsten.strauss@julius-kuehn.de5&G	*SmolkaSilvia4422silvia.smolka@julius-kuehn.de3%E	)SevenJasmin4454jasmin.seven@julius-kuehn.de9$K	(SchroeterAchim4435achim.schroeter@julius-kuehn.de9#K	'SchorppQuentin4452quentin.schorpp@julius-kuehn.de9"#K	&ScheidemannUta4621uta.scheidemann@julius-kuehn.de;!M	%SchamlottSabine4414sabine.schamlott@julius-kuehn.de5 G	$RoggeKerstin4447kerstin.rogge@julius-kuehn.de9K	#PfaffAlexander4412alexander.pfaff@julius-kuehn.de7I	"MitschkePetra4412petra.mitschke@julius-kuehn.de/A	!MerkerIna4405ina.merker@julius-kuehn.de?!Q	 LehmhusChristiane4404christiane.lehmhus@julius-kuehn.de5G	KuehneBianca4612bianca.kuehne@julius-kuehn.de3E	KrenzMarion4613marion.krenz@julius-kuehn.de8I	KoerberBaerbel4415barbel.koerber@julius-kuehn.de?#O	Koennecke_fKerstin4446kerstin.koennecke@julius-kuehn.de?#Q	KirchhammerKatrin4419katrin.kirchhammer@julius-kuehn.deK/]	Karolczak-KlekampMarion4416marion.karolczak-klekamp@julius-kuehn.de5G	JunkerCorina4429corina.junker@julius-kuehn.de>!K	JeworutzkiElke4435/4436elke.jeworutzki@julius-kuehn.de1C	IdczakElke4408elke.idczak@julius-kuehn.de5G	HommesMartin4400martin.hommes@julius-kuehn.de/A	HoeferUte4435ute.hoefer@julius-kuehn.de7I	HerbstMalaika4424malaika.herbst@julius-kuehn.deE-W	Heinrich-SiebersElke4410elke.heinrich-siebers@julius-kuehn.de3E	HauffeJulia4420julia.hauffe@julius-kuehn.de3E	GoetzMonika4403monika.goetz@julius-kuehn.de=O	GottfriedHenrike4417henrike.gottfried@julius-kuehn.de7I	FoersterAntje4612antje.foerster@julius-kuehn.de<
I	FeldmannFalko4406/3213falko.feldmann@julius-kuehn.de9	K	ErhardMichaela4614micheala.erhard@julius-kuehn.de9K	DresslerElvira4413elvira.dressler@julius-kuehn.de7I	DrechslerTina4426tina.drechsler@julius-kuehn.de7I	
BurlakKathrin4426kathrin.burlak@julius-kuehn.de;M		BraesickeNadine4602nadine.braesicke@julius-kuehn.de9K	BoeckmannElias4441Elias.Boeckmann@julius-kuehn.deA!S	BerendesKarl-Heinz4605karl-heinz.berendes@julius-kuehn.de9K	AchillesDoerte4426doerte.achilles@julius-kuehn.de8	K	GebeleinDieter4434dieter.gebelein@julius-kuehn.de