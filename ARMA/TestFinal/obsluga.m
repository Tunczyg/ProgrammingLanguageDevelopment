% zmie� typ na matlabowa tablice ze struktur
clear all;
close all;
clc;
load tab;
btab(:,:) = struct2cell(tab(:))
% transponuj
ctab = btab'
% wybierz tylko te, kt�rych jezyk to C#
dtab = ctab(strcmp(ctab(:,1), 'C++'),:);
dtab1=sortrows(dtab,2);
%wybierz rz�dy do liczenia korelacji
% corMat = B(:,2:5)
