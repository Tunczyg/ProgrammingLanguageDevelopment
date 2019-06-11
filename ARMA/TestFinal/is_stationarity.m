function [logic_value ] = is_stationarity( Y, xls )

% lags = (0:8)';
% [h,pValue,stats] = kpsstest(Y,'Lags',lags,'Trend',true);
% results = [lags pValue stats]
% fprintf('KPSS Test LAG 0:8\n');
% fprintf('h:\n');
% disp(h);
% disp(pValue);
% disp(stats);

fprintf('ADF test\n');
%h  = adftest(Y);

%h_adf  = adftest(Y, 'alpha',0.06 );

h_adf  = adftest(Y);
h_adf_1 = adftest(Y, 'model','ard', 'lags',2);
h_adf_2 = adftest(Y, 'model','TS', 'lags',2)

adf_test_h = adftest(Y, 'model','ard', 'lags',2)
% if 1 jest stacjonarny

% prosty test na stacjonarnosc
fprintf('adf test:\n');
if adf_test_h == 1
   fprintf('adf test -> series is a stationary, h:%d\n',adf_test_h);
   fprintf('indicates that there is enough evidence to suggest that series is a stationary\n');
   logic_value = adf_test_h;
else
    fprintf('adf test ->no evidence stationary, h:%d\n',adf_test_h);
    fprintf('indicates that there is no enough evidence to suggest that series is a stationary\n');
    logic_value = adf_test_h;
end



fprintf('\n');
% test na stacjonarnosc trendu
adf_test_h_trend = adftest(Y, 'model','ts', 'lags',2);
fprintf('adf trend test:\n');
if adf_test_h_trend == 1
   fprintf('adf test -> series is a trend stationary, h:%d\n',adf_test_h_trend);
   fprintf('indicates that there is enough evidence to suggest that series is a stationary\n');
else
    fprintf('adf test ->no evidence trend stationary, h:%d\n',adf_test_h_trend);
    fprintf('indicates that there is no enough evidence to suggest that series is a stationary\n');
end


kpss_test_h = kpsstest(Y, 'lags',2);

fprintf('kpss test:\n');
if kpss_test_h == 1
   fprintf('kpss test -> series is a stationary, h:%d\n',kpss_test_h);
   fprintf('indicates that there is enough evidence to suggest that series is a stationary\n');
else
    fprintf('kpss test ->no evidence stationary, h:%d\n',kpss_test_h);
    fprintf('indicates that there is no enough evidence to suggest that series is a stationary\n');
end
fprintf('\n');


kpss_test_h_trend = kpsstest(Y, 'lags',2, 'trend',true);
fprintf('kpss test:\n');
if kpss_test_h_trend == 1
   fprintf('kpss test -> series is a trend stationary, h:%d\n',kpss_test_h_trend);
   fprintf('indicates that there is enough evidence to suggest that series is a stationary\n');
else
    fprintf('kpss test ->no evidence trend stationary, h:%d\n',kpss_test_h_trend);
    fprintf('indicates that there is no enough evidence to suggest that series is a stationary\n');
end
fprintf('\n');



% hY1 = kpsstest(y1, 'lags',2, 'trend',true)
% hY2 = kpsstest(y2, 'lags',2, 'trend',true)
% hY3 = kpsstest(y3, 'lags',2)
% hY4 = kpsstest(y4, 'lags',2, 'trend',true)


% Hipotezy zerowa i alternatywna dla testu ADF s¹ nastêpuj¹ce:
% H0: Szereg jest niestacjonarny z powodu wystêpowania pierwiastka jednostkowego,
% H1: Szereg jest stacjonarny.
% Jeœli obliczona wartoœd statystyki

% KPSS test:
% 
% Null Hypothesis: the process is trend-stationary
% Alternative Hypothesis: the process has a unit root (this is how the authors of the test defined 
%the alternative in their original 1992 paper)
% ADF test:
% 
% Null Hypothesis: the process has a unit-root ("difference stationary")
% Alternative Hypothesis: the process has no unit root. It can mean either that 
%the process is stationary, or trend stationary, depending on which version of the ADF test is used.

	
% The Concepts and examples of Unit-root tests and stationarity tests
% 
% Concept of Unit-root tests:
% 
% Null hypothesis: Unit-root
% 
% Alternative hypothesis: Process has root outside the unit circle, which is usually equivalent to stationarity or trend stationarity
% 
% Concept of Stationarity tests
% 
% Null hypothesis: (Trend) Stationarity
% 
% Alternative hypothesis: There is a unit root.
% 
% There are many different Unit-root tests and many Stationarity tests.
% 
% Some Unit root tests:
% 
% Dickey-Fuller test
% Augmented Dickey Fuller test
% Phillipps-Perron test
% Zivot-Andrews test
% ADF-GLS test
% The most simple test is the DF-test. The ADF and the PP test are similar to the Dickey-Fuller test, but they correct for lags. The ADF does so by including them the PP test does so by adjusting the test statistics.
% 
% Some Stationarity tests:
% 
% KPSS
% Leybourne-McCabe
% In practice KPSS test is used far more often. The main difference of both tests is that KPSS is a non-parametric test and Leybourne-McCabe is a parametric test.
% 
% How unit-root test and stationarity-test complement each other
% 
% If you have a time series data set how it usually appears in econometric time series I propose you should apply both a Unit root test: (Augmented) Dickey Fuller or Phillips-Perron depending on the structure of the underlying data and a KPSS test.
% 
% Case 1 Unit root test: you can’t reject H0; KPSS test: reject H0. Both imply that series has unit root.
% 
% Case 2 Unit root test: Reject H0. KPSS test: don`t reject H0. Both imply that series is stationary.
% 
% Case 3 If we can’t reject both test: data give not enough observations.
% 
% Case 4 Reject unit root, reject stationarity: both hypothesis are component hypothesis – heteroskedasticity in series may make a big difference; if there is structural break it will affect inference.
% 
% Power problem: if there is small random walk component (small variance ? 2 u ), we can’t reject unit root and can’t reject stationarity.
% 
% Economics: if the series is highly persistence we can’t reject H0 (unit root) – highly persistent may be even without unit root but it also means we shouldn’t treat/take data in levels.
% 
% General rule about statistical testing You cannot proof a null hypothesis you can only affirm it. However if you reject a null hypothesis you can be very sure that the null hypothesis is really not true. Thus alternative hypothesis is always a stronger hypothesis than the null hypothesis.
% % 
% Variance ratio tests:
% 
% If we want to quantify how important the unit root is, we should use Variance Ratio Test.
% 
% In contrast to Unit root and Stationarity tests variance ratio tests can also detect the strenght of the unit root. Roughly the outcomes of a variance ratio test can be divided in 5 different groups.
% 
% Bigger than 1 After the shock the value of the variable explodes even more in the direction of the shock.
% 
% (Close to) 1 You get this value in the "classical case of a Unit root"
% 
% Between 0 and 1 After the shock the value approaches a level between the value before the shock and the value after the shock.
% 
% (Close to) 0 The series is (close to) stationarity
% 
% Negative After the shock the value goes into the opposite direction, i.e. if the value before the shock is 20 and the value after the shock is 10 on the long haul the variable will take on values greater than 20.