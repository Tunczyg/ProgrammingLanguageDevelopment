clear all;
close all;
clc;
% Wybór danych
y_out=xlsread('test1.xlsx')
Y=y_out(:,2);
T =  size(Y,1);
dates=y_out(:,1);
X=y_out(:,3:6);
size_series = size(Y,1);
factor = 0.15;
probe_learn = ceil(size_series * (1-factor));
probe_tests = size_series - probe_learn;
horizont=probe_tests+5;

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
[Ym,YMSEm] = forecast(EstMdl,horizont,'Y0',Y(1:probe_learn));
 

figure()
h1 = plot(Y(1:probe_learn),'Color',[.7,.7,.7]);
hold on;
% h2 = plot(probe_learn:size_series-1,Ym-Y(probe_learn),'b','LineWidth',2);
h2 = plot(probe_learn:probe_learn+horizont-1,Ym-Y(probe_learn),'b','LineWidth',2);
h3 = plot(probe_learn:size_series,Y(probe_learn:size_series),'g','LineWidth',2);
legend([h1 h2 h3],'Probe_learn','Prediction','Probe_tests','location','northwest');
grid on;
