function[]=Future_Prediction(LanguageName,Factor,Horizont,Name,PopularitySurvey_val,PullRequests_val,Publications_val)
%Future_Prediction
 % INPUTS:
 % LangaugeName - Wyb�r j�zyka
 % Factor - podzia� na probe_learn(100%-Factor%) i probe_tests(Factor %)
 % Horizont - horyzont do przewidywania- ile lat do przodu
 % Name - Nazwa pliku do kt�rego zostanie zapisany wykres wyj��iow
 % PopularitySurvey_val - Wsp_1
 % PullRequests_val -  Wsp_2
 %  Publications_val - Wsp_2
 %
 %
 % Popularno��= Wsp_1*Stack+Wsp_2*GitHub +Wsp_3*Publikacje
path = pwd + "\jpegi\";
load tab.mat;
btab(:,:) = struct2cell(tab(:))
% transponuj
ctab = btab'
% wybierz j�zyk
 dtab = ctab(strcmp(ctab(:,1), LanguageName),:);
dtab=sortrows(dtab,2);
dates=cell2mat(dtab(:,2)); %daty
A_1=cell2mat(dtab(:,3));
B_1=cell2mat(dtab(:,4));
C_1=cell2mat(dtab(:,5)); % Wybor odpowiednich znanych warto�ci
A_2=Arimax_Prediction3(LanguageName,Factor,Horizont,3,'Test1.jpg');
B_2=Arimax_Prediction3(LanguageName,Factor,Horizont,4,'Test2.jpg');
C_2=Arimax_Prediction3(LanguageName,Factor,Horizont,5,'Test3.jpg'); % Przewidywanie dla kolejnych wartosci
Data_1=PopularitySurvey_val*A_1+PullRequests_val*B_1+Publications_val*C_1; %Utworzenie Popularnosci dla znanych danych
Data_2=PopularitySurvey_val*A_2(:,2)+PullRequests_val*B_2(:,2)+Publications_val*C_2(:,2); %Utworzonenie popularnosci 
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
size_series = size(Data_1,1);
% Factor = 0.15;
probe_learn = ceil(size_series * (1-Factor))
delta=Data_1(probe_learn)-Data_2(1);

dates_2=[A_2(2:end,1);max(A_2(:,1))+1];
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