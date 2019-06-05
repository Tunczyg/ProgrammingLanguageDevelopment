 function[Future]=Arimax_Prediction3(LanguageName,Factor,Horizont,Column,Name)
 % Arimax_Prediction3
 %
 % INPUTS:
 % LangaugeName - Wybór jêzyka
 % Factor - podzia³ na probe_learn(100%-Factor%) i probe_tests(Factor %)
 % Horizont - horyzont do przewidywania- ile lat do przodu
 % Column - Wybór kolumny zawieraj¹cej zmienn¹ objaœnian¹ [3:Iloœæ zmiennych)
 % Name - Nazwa pliku do którego zostanie zapisany wykres wyjœæiow
 %
 %OUPTPUTS:
 % Future- macierz o kolumnach z datami i przepowiedzianymi wartoœciami
 % Przyk³ad:A=Arimax_Prediction3('C#',0.15,7,4,'Test2.jpg')
 %
 %UWAGI:
 %Do u¿ycia konieczne jest posiadanie tab.mat oraz fit_arimax.m
 %Factor- Dzia³a z max 0.15, co daje dwie próbki testowe
 
 
load tab.mat;

btab(:,:) = struct2cell(tab(:))
% transponuj
ctab = btab'
% wybierz jêzyk
 dtab = ctab(strcmp(ctab(:,1), LanguageName),:);
dtab=sortrows(dtab,2);

% dtab = ctab(strcmp(ctab(:,1), 'PHP'),:);
%wybierz rzêdy do liczenia korelacji
% corMat = B(:,2:5)



Y=cell2mat(dtab(:,Column));
dates=cell2mat(dtab(:,2));
X_1=cell2mat(dtab(:,3:Column-1));
X_2=cell2mat((dtab(:,Column+1:end)));
X=[X_1 X_2];
i=1;
T=size(Y,1);
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
%Dodanie wartoœci na pocz¹tek wektorów
    while(T<15)
    Y=[1;Y];
    dates=[min(dates)-1;dates];
    X=[[1,1];X];
    % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
    T =  size(Y,1);
    end

size_series = size(Y,1);
% Factor = 0.15;
probe_learn = ceil(size_series * (1-Factor));
probe_tests = size_series - probe_learn;
% Horizont=probe_tests+5;


dates_2=dates(probe_learn:T);
    for i=1:Horizont-probe_tests-1
        dates_2=[dates_2;max(dates)+i];
    end

% Estymowanie dla ARIMAX- na podstawie matlab works
% y0 = [Y(1);Y(2);Y(3);Y(4)];
yEst = Y(2:T);
XEst = X(2:end,:);
Beta0 = [0.5 0.5 0.5 0.5];
%Dobranie parametrów
[p, d, q, bic, aic, fit,EstParamCov,logL,info ] = fit_arimax_model( Y,probe_learn, probe_tests )
%Utworzenir modelu
Mdl=arima(fit.P,fit.D,fit.Q);
% Estymowanie modelu
% y0 = Y(1:fit.P+1);
 y0=[Y(size_series-3);Y(size_series-2);Y(size_series-1);Y(size_series)];
%y0 = y0';
EstMdl = estimate(Mdl,yEst,'X',XEst,'Y0',y0);
%Predykcja modelu
[Ym,YMSEm] = forecast(EstMdl,Horizont,'Y0',Y(1:probe_learn));

 

a=figure('visible','off')
h1 = plot(dates(1:probe_learn),Y(1:probe_learn),'-*','Color',[.7,.7,.7]);
hold on;
% h2 = plot(probe_learn:size_series-1,Ym-Y(probe_learn),'b','LineWidth',2);
% h2 = plot(dates_2(probe_learn:probe_learn+Horizont-1),Ym-Y(probe_learn),'b','LineWidth',2);

h2 = plot(dates_2,Ym,'-*b','LineWidth',2);


h3 = plot(dates(probe_learn:size_series),Y(probe_learn:size_series),'-*g','LineWidth',2);
legend([h1 h2 h3],'Probe learn','Prediction','Probe tests','location','northwest');
saveas(a,Name);
grid on;
Future=[dates_2 Ym]

 end