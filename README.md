# ProgrammingLanguageDevelopment  
  
**Cel projektu:**  
  
Będąc studentami trzeciego roku stajemy przed wyborem praktyk studenckich, których zaliczenie jest obowiązkowe do września bieżącego roku. Aby pomóc naszym kolegom świadomie wybrać kierunek zatrudnienia postanowiliśmy zaprognozować rozwój języków programowania w kolejnych kwartałach. Do analizy użyto danych ze stron internatowych o charakterze programistycznym, ekonomicznym oraz ogólnonaukowym. Z uwagi na fakt, że dane wcześniejsze niż rok 2012 są niedostępne lub szczątkowe, ograniczono punkty pomiarowe do cokwartalnych statystyk od Q1 2012 do Q2 2019, co daje 30 punktów pomiarowych. Horyzont symulacji to 8 punktów pomiarowych, z czego dla wskazania poprawności algorytmu 6 należy do zbioru testowego.  
Prezentację multimedialną z projektu można znaleźć pod adresem: https://prezi.com/view/CnkxSZ50TUMLKpZLVS30/
  
**Opis aplikacji:**  
  
Cała aplikacja jako wersja developerska służąca symulacji korzysta zarówno ze środowiska Visual Studio, jak i Matlab. Z tego powodu, że nie każdy użytkownik posiada w tym samym systemie operacyjnym oba te programy w dokumentacji prezentujemy wersję uproszczoną, której można używać nawet posiadając te dwa środowiska na oddzielnych urządzeniach.  
  
Głównym branchem dla ProgrammingLanuageDevelopment jest MatlabRepository, reszta branchy (poza masterem) to wersje testowe lub developerskie. Do poprawnego pobrania danych konieczne jest stabilne i najlepiej szybkie łącze internetowe. Wszelkie zerwania połączenia skutkują koniecznością ponownego uruchomienia programu.  

ProgrammingLanguageDevelopment jest to aplikacja typu konsolowego z prostym interefejsem komunikacyjnym.  
Ma na celu pobieranie danych statystycznych z różnych stron internetowych, aby mogły one posłużyć do symulacji rozwoju języków przy użyciu modelu symulacyjnego ARIMAX.

Wygląd interfejsu komunkacyjnego:  
![alt text](https://github.com/Tunczyg/ProgrammingLanguageDevelopment/blob/MatlabReference/Documentation/ConsoleApp.PNG)
  
  
    
Pliki wynikowe są zapisywane w ścieżce ProgrammingLanguageDevelopment\bin\Debug i noszą nazwy formatu: "rok.txt".  
Format pojedynczego rekordu:  
*{  
&nbsp;&nbsp;&nbsp;&nbsp; "LanguageName":string,  
&nbsp;&nbsp;&nbsp;&nbsp; "Year":4_digit_number,  
&nbsp;&nbsp;&nbsp;&nbsp; "Quarter":1_digit_number,  
&nbsp;&nbsp;&nbsp;&nbsp; "PopularitySurvey":percentage,  
&nbsp;&nbsp;&nbsp;&nbsp; "PullRequestsAmount":number,  
&nbsp;&nbsp;&nbsp;&nbsp; "PushAmount":number,  
&nbsp;&nbsp;&nbsp;&nbsp; "StarsAmount":number,  
&nbsp;&nbsp;&nbsp;&nbsp; "IssuesAmount":number,  
&nbsp;&nbsp;&nbsp;&nbsp; "PublicationsAmount":number  
}*  
  
Następnie za pomocą funkcji convert.m konwertowane są do postaci .mat, którą należy dostarczyć do folderu znajdującego się w branchu master o ścieżce ARMA/TestFinal/. Funkcja główna to Future_Prediction.m, która (pośrednio) wywołuje najważniejszą w projekcie funkcję - fit_arimax_model.m. Idea działania modelu:  
  
Używany jest model ARIMAX, który tworzony jest z modelu ARIMA. Jego najważniejszymi parametrami są :  
    • P- część od AR- parametr autoregresji  
    • Q- część od MA-parametr średniej ruchomej  
    • D-  dla ARMAX – parametr od różniczkowania (najczęściej 1)- stopień szeregu.
 
Wyniki symulacji znajdują się na dysku pod linkiem:  
https://drive.google.com/drive/folders/1CxKgl_cDAy4KncA8YitUkL3ghvWc1xqO?usp=sharing

Zestawienie wyników oraz wnioski zostają przedstawione w poniższej tabeli:  
![alt text](https://github.com/Tunczyg/ProgrammingLanguageDevelopment/blob/MatlabReference/EconomicStatData/Tabela.png)
  
**Komentarz:**  
  
Skuteczność algorytmu w dużej mierze zależy od ilości danych statystycznych opisujących środowisko rzeczywiste. Dla idealnego przypadku, który małby skuteczność 100% baza danych byłaby bazą nieskończoną, z całkowitą dokładnością odzwierciedlającą sytuację ekonomiczno-społeczną na rynku. Z powodu ograniczonego czasu i zasobów do bazy danych projektu zostały włączone jedynie wybrane zestawy danych, które niestety z oczywistych względów nie modelują wiernie rzeczywistości. Mając na uwadze niską liczbę rekordów w bazie skuteczność algorytmu jest bardzo wysoka i można polecić go do rozwiązywania problemów, dla których ma się wiedzę ekspercką pozwalającą na dostarczenie odpwiednio bogatej bazy danych.  
Projekt potwierdził nasze przewidywania, że co do większości języków ciężko jest stwierdzić z całą pewnością czy ich opularność będzie wzrastała, jednak Javascript mający od lat tendencę wzrostową nadal pozostanie na korzystnej pozycji. Z tego powodu studentom wahającym się co do wyboru języka, w którym będą pracować na praktykach wakacyjnych poleca się zainteresowanie Javascriptem.
