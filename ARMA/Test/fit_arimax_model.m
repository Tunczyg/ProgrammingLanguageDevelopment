function [ p, d, q, bic, aic, fit,EstParamCov,logL,info ] = fit_arimax( Y,probe_learn, probe_tests )
%FIT_ARMA Summary of this function goes here
%   Detailed explanation goes here
 
% P = 4;
% % d = 0;
% % q = 4;
% from 

Y_org = Y
Y=Y(1:probe_learn,1);
% zmiejszamy Y do zbioru uczacego



%set max
P_= 0;
D_= 1;
Q_= 0;

%set min
P__= 2;
D__= 1;
Q__= 2;

Px = P__-P_+1;
Qx = Q__-Q_+1;

%stationary test
%hip = st_test(Y);
hip= is_stationarity(Y);
%hip=0;
% if 0 -> non stationary
if hip == 1 
    
    d = 0;
else
    d = 1;
end

%To identify the best lags, fit several models with different lag choices. Here, 
% fit all combinations of p = 1,...,4 and q = 1,...,4 (a total of 16 models). Store the loglikelihood objective function and number of coefficients for each fitted model.

% Plot the sample ACF and PACF.

% figure;
% subplot(2,1,1)
% autocorr(Y)
% subplot(2,1,2)
% parcorr(Y)

% Fit ARMA(p,q) models.

LOGL = zeros(Px,Qx); %Initialize
PQ = zeros(Px,Qx);
% p_=0;
% q_=0;

% set min


for p_ = P_+1:P__+1
    for q_ = Q_+1:Q__+1
        

        p = p_-1;
        q = q_-1;
        fprintf('\n');
        fprintf('****************************\n');
        fprintf('model (%d, %d, %d) \n', p, d, q);
        if (p+q)>0
        % nie mo¿e byc p - q =0
            mod = arima(p, d , q);
            %[fit,EstParamCov,logL,info] = estimate(mod,Y,'print',false);
            %[fit,EstParamCov,logL,info] = estimate(mod,Y);
            %[fit,EstParamCov,logL,info] =  estimate(mod,Y,'print',true,'Y0',Y(1:3,1), 'Display',{'params','diagnostics', 'iter', 'full'});
            %wejsciowe dane do optymalizacji
            [fit,EstParamCov,logL,info] =  estimate(mod,Y(1:probe_learn),'display','full','Y0',Y(1:3,1), 'Display',{'params','diagnostics', 'iter', 'full'});
            LOGL(p_,q_) = logL;
            PQ(p_,q_) = (p)+(q);
            fprintf('model:LoGL %d \n',logL);
            fprintf('Variance-covariance matrix of maximum likelihood  \n');
            disp(EstParamCov);
            
            %[aic1,bic1] = aicbic(LOGL,PQ+1,100);
            %fprintf('model:aic: %d bic %d \n',aic1,bic1);
            fprintf('model \n');
            disp(fit);
            print(fit,EstParamCov);
            
        else  
            LOGL(p_,q_) = -1000000000000000000000000000000000;
            PQ(p_,q_) = (p)+(q);
        end
         
            fprintf('model (%d, %d, %d) \n', p, d, q);
            fprintf('*************\n');
     end
end

fprintf('Disp LOGL\n');
%disp(LOGL);

% disp

for i=1:size(LOGL,1)
    for y=1:size(LOGL,2)
        fprintf('%d ', LOGL(i,y))
    end
    fprintf('\n');
end


% Calculate the BIC.
siz = size(LOGL,1)*size(LOGL,2);
s_1 = size(LOGL,1);
s_2 = size(LOGL,2);

LOGL = reshape(LOGL,siz,1);
PQ = reshape(PQ,siz,1);
%[~,bic] = aicbic(LOGL,PQ+1,100);
[aic,bic] = aicbic(LOGL,PQ+1,size(Y,1));
%[aic,bic] = aicbic(LOGL,2,size(Y,1));
bic_matrix = reshape(bic, s_1,s_2);
aic_matrix = reshape(aic, s_1,s_2);
 fprintf('model bic \n');
 disp(bic_matrix);
 for i=1:size(bic_matrix,1)
    for y=1:size(bic_matrix,2)
        fprintf('%d ', bic_matrix(i,y))
    end
    fprintf('\n');
end
  fprintf('model aic \n');
disp(aic_matrix);
 for i=1:size(aic_matrix,1)
    for y=1:size(aic_matrix,2)
        fprintf('%d ', aic_matrix(i,y))
    end
    fprintf('\n');
end
%index of p and q -> min matrix
[p1,q1] = find(bic_matrix==min(bic_matrix(:)));


% if model start from 0 p_min = 0
p = p1-1;
q = q1-1;
fprintf('Bic: \n');
%disp(bic);


%


% estymuje jeszcze raz model

mod = arima(p, d , q);
[fit,EstParamCov,logL,info] = estimate(mod,Y,'display','off');

bic = bic_matrix(p1,q1);
aic = aic_matrix(p1,q1);
 fprintf('optimal structure of arimax (%d, %d, %d),     bic best: %d aic best: %d \n', p, d, q, bic_matrix(p1,q1), aic_matrix(p1,q1));
%In the output BIC matrix, the rows correspond to the AR degree (p) and the columns correspond to the MA degree (q). The smallest value is best.
%The smallest BIC value in the (r,c) position. This corresponds to an ARMA(x,y) model, matching the model that generated the data.

%przywracamy Y do oryginalu
Y = Y_org
end

