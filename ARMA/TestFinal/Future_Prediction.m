function[]=Future_Prediction(LanguageNumber,Factor,Horizont,Name,Wsp1,Wsp2,Wsp3,Wsp4,Wsp5,Wsp6)
%Future_Prediction
 % INPUTS:
 % LangaugeName - Wybór jêzyka
 % Factor - podzia³ na probe_learn(100%-Factor%) i probe_tests(Factor %)
 % Horizont - horyzont do przewidywania- ile lat do przodu
 % Name - Nazwa pliku do którego zostanie zapisany wykres wyjœæiow
 % Wsp1 - Wsp_1
 % Wsp2 -  Wsp_2
 % Wsp3 - Wsp_2
 % Ws
 %
 %
 % Popularnoœæ= Wsp_1*Stack+Wsp_2*GitHub +Wsp_3*Publikacje
path = pwd + "\jpegi\";
load BigTabRouded.mat;
load slownik.mat
dtab=BigTabRouded(BigTabRouded(:,1)==LanguageNumber,:);
dtab=abs(dtab);
dates=dtab(:,2);
LanguageName=slownik(LanguageNumber);

% A_1=dtab(:,3);
B_1=dtab(:,4);
C_1=dtab(:,5); % Wybor odpowiednich znanych wartoœci
D_1=dtab(:,6);
E_1=dtab(:,7);
F_1=dtab(:,8);
A_2=Arimax_Prediction3(LanguageNumber,Factor,Horizont,3,'Test1.jpg');
B_2=Arimax_Prediction3(LanguageNumber,Factor,Horizont,4,'Test2.jpg');
C_2=Arimax_Prediction3(LanguageNumber,Factor,Horizont,5,'Test3.jpg');
D_2=Arimax_Prediction3(LanguageNumber,Factor,Horizont,6,'Test4.jpg');
E_2=Arimax_Prediction3(LanguageNumber,Factor,Horizont,7,'Test5.jpg');
F_2=Arimax_Prediction3(LanguageNumber,Factor,Horizont,8,'Test6.jpg');
% Przewidywanie dla kolejnych wartosci
Data_1=Wsp2*B_1+Wsp3*C_1+Wsp4*D_1+Wsp5*E_1+Wsp6*F_1; %Utworzenie Popularnosci dla znanych danych
Data_2=Wsp2*B_2(:,2)+Wsp3*C_2(:,2)+Wsp4*D_2(:,2)+Wsp5*E_2(:,2)+Wsp6*F_2(:,2); %Utworzonenie popularnosci 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
size_series = size(Data_1,1);
% Factor = 0.15;
probe_learn = ceil(size_series * (1-Factor))
probe_tests=size_series-probe_learn
if(probe_tests==0)
delta=Data_1(probe_learn-1)-Data_2(1);
else
    delta=Data_1(probe_learn)-Data_2(1);
end

dates_2=[B_2(2:end,1);max(B_2(:,1))+0.25];
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% dla przewidzianych danych
a=figure('visible','off');
h1=plot(dates,Data_1,'-b*');
hold on;
h2=plot(dates_2,Data_2+delta,'-r*');
legend([h1 h2],{'Data','Prediction'},'location','northwest')
title(LanguageName);
grid on;
saveas(a,fullfile(path,Name));

end