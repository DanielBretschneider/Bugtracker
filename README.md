```bat
Code wird zur Zeit im Master-Branch entwickelt.
```

# Bugtracker
Der Bugtracker dient in erster Linie der raschen Fehlerdokumentation und erleichtert die Analyse und Identifikation bei Softwareproblematiken. Ausgeführt als Applikation mit 
grafischem Interface, oder via CMD/Terminal sammelt der Bugtracker Logdateien der jeweiligen Software und speichert sie auf einem Shared-Folder im Unternehmensnetzwerk. 
Zusätzlich dazu sollen Screenshots der Anwendung und der Fehlermeldung automatisiert ausgelöst werden. Die gesammelten Logdateien, wie die Screenshots, werden zu einer .ZIP-Datei 
komprimiert und zur weiteren Analyse gespeichert. 

Die Erstversion des Bugtrackers ist bereits erschienen und seit geraumer Zeit im Einsatz, jedoch ist hiervon nur noch die ausführbare Datei vorhanden, der Quellcode ging verloren. 
Deshalb wurde beschlossen eine neue Version des Bugtrackers zu entwickeln und neue Features hinzuzufügen. Beispielsweise soll der Bugtracker nun auch (als Agent) aus der Distanz 
(remote) ausgeführt werden können, ohne dass der Benutzer auf dem jeweiligen PC tätig werden muss. Der Bugtracker grundsätzlich auf Windowsumgebungen ausgeführt werden, aber auch 
Kompatibilität mit Linux-Systemen vorweisen, um auch hier Log- und Protokolldateien automatisiert sammeln zu können. 

## Entwicklung

Der Bugtracker wurde in Visual Basic programmiert und über einen One-Click-Installer installiert. Die grafische Oberfläche oben den Hostnamen des PCs an, darunter die aktuelle Uhrzeit inklusive des Datums. Unter der Zeitanzeige ist die Problembeschreibung angesiedelt, bestehend aus einem Dropdown-Menü mit folgenden Problemkategorien .
Die Problemkategorien wurde erstellt, damit ein wiederkehrendes Fehlverhalten nicht jedes Mal aufs Neue im darunterliegenden Textfeld, oder telefonisch, beschrieben bzw. erklärt werden muss und um den Rahmen bei der Fehlersuche einzugrenzen. Im Textfeld können Kommentare zur Problematik gemacht werden.
In der Programmsektion kann die bzw. können die fehlerhaften Programme ausgewählt werden. Die Logdateien der Programme deren Checkbox aktiv ist werden gesammelt. 
Mittels „Aufzeichnen und Beenden“ werden die Logdateien eingeholt, die Screenshots ausgeführt und alle Dateien samt Bugtracker-Logdatei im Netzwerkordner gespeichert. 
Bisher definierte Problemkategorien im Bugtracker v1:
*	Allgemeiner Fehler
*	Syngo Via Bilder fehlen
*	IP8 langsam
*	Bilder fehlen
*	Bilder Benennung falsch
*	IP8 Hanging
*	XR reagiert nicht
*	Word Problem 
*	TH langsam
*	SMS-Problem


### Funktionen des neuen Bugtrackers
Die Zweitversion des Bugtrackers wird in verschiedenen Unterversionen entwickelt, diese werden im Kapitel Entwicklung noch näher erläutert. Folgende Funktionalitäten werden im neuen Bugtracker umgesetzt:
*	Konfiguration des Bugtrackers erfolgt über XML-Datei
  -	Speichert Pfade der Softwarekomponenten
  -	Speichert Pfad zur Ablage des komprimierten Ordners
  -	Logging aktivieren oder deaktivieren
  -	Screenshots aktivieren oder deaktivieren

*	Logdatei anhand des Pfades finden und mit Überordner kopieren
  - Nur die letzten 2000 Zeilen (bzw. variabler Wert, Angabe über XML-Konfigurationsdatei), da in diesem Bereich in der Mehrheit der Fälle die gesuchten Informationen zu finden sind.

*	Aufnahme aller Monitore (Screenshots)
  - Damit auch grafische Fehlerinformationen in GUI-Fenstern bzw. Informationen zur aktuellen Tätigkeit mitgeliefert werden können.

*	Der neue Bugtracker soll via Command Line angesprochen und ausgeführt werden können

*	Der Bugtracker soll als Applikation mit grafischem Interface (Stichwort: Benutzerfreundlichkeit) bedient werden können

*	Als Agent soll die Neuversion nun auch remote ausgeführt werden können, ohne die Vorgänge der auf dem PC arbeitenden Person zu beeinträchtigen.

*	Prinzipiell wird der Bugtracker für die Anwendung auf Windowssystemen optimiert, auch weil er mittels C# entwickelt wird. Es soll dennoch eine grundsätzliche Kompatibilität zu Linux-Umgebungen geschaffen werden, um auch hier Logdateien und Ähnliches bekommen zu können.

*	Die Programmsektion soll dynamisch generiert werden. Das bedeutet, dass Applikationen, die nicht am lokalen PC installiert sind, nicht zur Auswahl stehen.

*	Dieses Programm soll in weiterer Folge als Open-Source-Projekt auf GitHub zur Verfügung stehen und so programmiert werden, dass auch andere Firmen, IT-AdministratorenInnen, EntwicklerInnen und LoganalystInnen damit arbeiten können.

*	Der Installer soll als Click-Once-Installer zur Verfügung stehen. Die Applikation selbst soll nur alle 14 Tage nach Updates suchen und diese installieren. 


### Versionierung

#### Version 2.1
In der Erstversion des Bugtrackers steht Grundfunktionalität im Vordergrund, dies inkludiert:
*	Fertiges Grundgerüst (Programmstruktur)
*	Fertige Konfigurationsdatei
*	Fertigstellung der Grundfunktionalität in Modulen
  - fetchLogfile() – Methode zum kopieren eines Logfiles in Zusammenarbeit mit der XML-Konfiguration  
  - captureMonitors() – Methode zum screencapturing
  - Logging
*	Command Line Befehl zur Steuerung des Bugtrackers


#### Version 2.2
Die Zweitversion erweitert den Bugtracker um Folgende Funktionalitäten und Inhalte: 
* Bedienung via GUI möglich
* Dynamische Programmsektion


#### Version 2.3
Ab dieser Version sollen Bugtracks auch remote durchgeführt werden auf den jeweiligen PC vom dem der Bugtrack gemacht werden soll zugreifen zu müssen. 

Der Agent soll automatisch auf den PCs gestartet werden und auf einem spezifischen Port hören. Dies wird in der Sektion Technische Spezifikationen noch erläutert.



