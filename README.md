# ProgrammingLanguageDevelopment  
  
Cel projektu:  
  
Będąc studentami trzeciego roku stajemy przed wyborem praktyk studenckich, których zaliczenie jest obowiązkowe do września bieżącego roku. Aby pomóc naszym kolegom świadomie wybrać kierunek zatrudnienia postanowiliśmy zaprognozować rozwój języków programowania w kolejnych kwartałach. Do analizy użyto danych ze stron internatowych o charakterze programistycznym, ekonomicznym oraz ogólnonaukowym. Z uwagi na fakt, że dane wcześniejsze niż rok 2012 są niedostępne lub szczątkowe, ograniczono punkty pomiarowe do cokwartalnych statystyk od Q1 2012 do Q2 2019, co daje 30 punktów pomiarowych. Horyzont symulacji to 8 punktów pomiarowych, z czego dla wskazania poprawności algorytmu 6 należy do zbioru testowego.

Opis aplikacji:  
  
Cała aplikacja jako wersja developerska służąca symulacji korzysta zarówno ze środowiska Visual Studio, jak i Matlab. Z tego powodu, że nie każdy użytkownik posiada w tym samym systemie operacyjnym oba te programy w dokumentacji prezentujemy wersję uproszczoną, której można używać nawet posiadając te dwa środowiska na oddzielnych urządzeniach.  
  
Głównym branchem dla ProgrammingLanuageDevelopment jest MatlabRepository, reszta branchy (poza masterem) to wersje testowe lub developerskie. Do poprawnego pobrania danych konieczne jest stabilne i najlepiej szybkie łącze internetowe. Wszelkie zerwania połączenia skutkują koniecznością ponownego uruchomienia programu.  

ProgrammingLanguageDevelopment jest to aplikacja typu konsolowego z prostym interefejsem komunikacyjnym.  
Ma na celu pobieranie danych statystycznych z różnych stron internetowych, aby mogły one posłużyć do symulacji rozwoju języków przy użyciu modelu symulacyjnego ARIMAX.

Wygląd interfejsu komunkacyjnego:  
![alt text](https://github.com/Tunczyg/ProgrammingLanguageDevelopment/blob/MatlabReference/Documentation/ConsoleApp.PNG)
  
  
Pliki wynikowe są zapisywane w ścieżce ProgrammingLanguageDevelopment\bin\Debug i noszą nazwy formatu: "rok.txt".  
Format pojedynczego rekordu:  
{  
&nbsp;&nbsp; "LanguageName":string,  
&nbsp;&nbsp; "Year":4_digit_number,  
&nbsp;&nbsp; "Quarter":1_digit_number,  
&nbsp;&nbsp; "PopularitySurvey":percentage,  
&nbsp;&nbsp; "PullRequestsAmount":number,  
&nbsp;&nbsp; "PushAmount":number,  
&nbsp;&nbsp; "StarsAmount":number,  
&nbsp;&nbsp; "IssuesAmount":number,  
&nbsp;&nbsp; "PublicationsAmount":number  
}  
  
Następnie za pomocą funkcji convert.m konwertowane są do postaci .mat, którą należy dostarczyć do folderu znajdującego się w branchu master o ścieżce ARMA/TestFinal/. Funkcja główna to Future_Prediction.m, która (pośrednio) wywołuje najważniejszą w projekcie funkcję - fit_arimax_model.m. Idea działania modelu:  
  
Używany jest model ARIMAX, który tworzony jest z modelu ARIMA. Jego najważniejszymi parametrami są :  
    • P- część od AR- parametr autoregresji  
    • Q- część od MA-parametr średniej ruchomej  
    • D-  dla ARMAX – parametr od różniczkowania (najczęściej 1)- stopień szeregu.
 
Wyniki symulacji znajdują się na dysku pod linkiem:  
https://drive.google.com/drive/folders/1CxKgl_cDAy4KncA8YitUkL3ghvWc1xqO?fbclid=IwAR177hFU3AGVJ_-MZEam-7ST64HCSQWmRxb6k8YbYGh2XIzIH08H2jlf1mM

Zestawienie wyników oraz wnioski zostają przedstawione w poniższej tabeli:  
![alt text](https://github.com/Tunczyg/ProgrammingLanguageDevelopment/blob/MatlabReference/EconomicStatData/Tabela.png)
