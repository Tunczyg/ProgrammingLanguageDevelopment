 function[Future]=Arimax_Prediction3(LanguageNumber,Factor,Horizont,Column,Name)
 % Arimax_Prediction3
 %
 % INPUTS:
 % LangaugeNumber - Wybór numeru jêzyka
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
 
 
load BigTabRouded.mat;
load slownik.mat
dtab=BigTabRouded(BigTabRouded(:,1)==LanguageNumber,:);
dtab=abs(dtab);
Y=dtab(:,Column);
dates=dtab(:,2);

LanguageName=slownik(LanguageNumber);



% dtab = ctab(strcmp(ctab(:,1), 'PHP'),:);
%wybierz rzêdy do liczenia korelacji
% corMat = B(:,2:5)


% Y=cell2mat(dtab(:,Column));
% dates=cell2mat(dtab(:,2));
% X_1=dtab(:,3:Column-1);
% X_2=dtab(:,Column+1:end);

X=[dtab(:,4:Column-1) dtab(:,Column+1:end)];

i=1;
T=size(Y,1);
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
%Dodanie wartoœci na pocz¹tek wektorów
    while(T<15)
    Y=[1;Y];
    dates=[min(dates)-0.25;dates];
    X=[[1,1];X];
    % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
    T =  size(Y,1);
    end
    
%Sprawdzenie czy macierz X ma czysto zerowe kolumny
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
for i=1:size(X,2)
    if nnz(X(:,i))<15
        X(size(X,1)-15:end,i)=1; % Jeœli mniej ni¿ 5 niezerwoych elementów to dodajemy 1 na ostatnie 5 pozycji
    end
end
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% if(nnz(X(:,i))==0)
%     X(:,i)=1;
% end

%Sprawdzenie czy macierz Y jest zerowa
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
for i=1:size(Y,1)
    if nnz(Y)<15
        Y(size(X,1)-15:end)=1; % Jeœli mniej ni¿ 5 niezerwoych elementów to dodajemy 1 na ostatnie 5 pozycji
    end
end
% % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % % 
% if nnz(Y)==0
%     Y(:)=1;
% end


size_series = size(Y,1);

probe_learn = ceil(size_series * (1-Factor));
probe_tests = size_series - probe_learn;

if(T-probe_learn>Horizont)
    dates_2=dates(probe_learn:probe_learn+Horizont-1)
else
dates_2=dates(probe_learn:T);
    for i=1:Horizont-probe_tests-1
        dates_2=[dates_2;max(dates)+i*0.25];
    end
end
% Estymowanie dla ARIMAX- na podstawie matlab works
% y0 = [Y(1);Y(2);Y(3);Y(4)];
yEst = Y(2:T);
XEst = X(2:end,:);
Beta0 = [0.5 0.5 0.5 0.5];
%Dobranie parametrów
[p, d, q, bic, aic, fit,EstParamCov,logL,info ] = fit_arimax_model( Y,probe_learn, probe_tests );
%Utworzenir modelu
Mdl=arima(fit.P,fit.D,fit.Q);
% Estymowanie modelu
% y0 = Y(1:fit.P+1);
 y0=[Y(size_series-3);Y(size_series-2);Y(size_series-1);Y(size_series)];
%y0 = y0';
EstMdl = estimate(fit,yEst,'X',XEst,'Y0',y0);
%Predykcja modelu
[Ym,YMSEm] = forecast(EstMdl,Horizont,'Y0',Y(1:probe_learn));
Ym=0.7*Ym;
Ym=abs(Ym);
% delta=Y(probe_learn)-Ym(1)


 

a=figure('visible','off')
h1 = plot(dates(1:probe_learn),Y(1:probe_learn),'-*','Color',[.7,.7,.7]);
hold on;


h2 = plot(dates_2,Ym,'-*b','LineWidth',2);


h3 = plot(dates(probe_learn:size_series),Y(probe_learn:size_series),'-*g','LineWidth',2);
legend([h1 h2 h3],'Probe learn','Prediction','Probe tests','location','northwest');
saveas(a,Name);
grid on;
Future=[dates_2 Ym];

 end