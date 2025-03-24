2025.03

En simpel WPF som har én Firkant som kan flyttes med musen. Der er også TCP connection, så man
kan starte 2 af disse op, og forbind. MEN opgaven som man skal bruge denne kode til er at vise 2 firkanter, en pr. computer, 
således at når clienten flytter firkanten med sit eget navn, så ses dette også hos server, og når server flytter sin firkant med sit eget 
navn, så ses dette hos clientet. Dvs. at begge viser begge firkanter. Her ser du hvordan den udleverede kode ser ud.

Her kører jeg 2 gange exe filen som ligger i bin folderen, alt på min computer. Vi ønsker at løsningen skal kører på 2 computere, hvor en er
server og den anden er client. Og så skal man kunne se begge firkanter på begge computere, og server skal kunne flytte sin firkant, og så flyttes
den også hos clienten og det samme gælder også den anden vej, fra clienten.

Her er nogle ting man skal overveje.
1. Først skal du og den anden i gruppen forbinde server og client, fra hver sin computer.
2. Så skal man overveje hvilke informationer der skal sendes og hvordan man læser denne når man modtager den (det kaldes en protokol).
3. Så må man overveje hvordan man laver koden således at den hele tiden lytter på nye inputs (kræver en tråd).
4. ....osv. der er mange ting at overveje og de behøves ikke at gøres i den rækkefølge jeg har skitseret her, det er bare en "komme igang" liste.

<img width="944" alt="image" src="https://github.com/user-attachments/assets/5a3af35c-12d2-4b2f-8371-1194be72797a" />
