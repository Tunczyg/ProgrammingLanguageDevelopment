clear all;
close all;
clc;

load tab.mat;2
btab(:,:) = struct2cell(tab(:))
ctab = btab'
dtab = ctab(strcmp(ctab(:,1),'PHP'),:);
dtab=sortrows(dtab,2);
dates=cell2mat(dtab(:,2));
A_1=cell2mat(dtab(:,3));
B_1=cell2mat(dtab(:,4));
C_1=cell2mat(dtab(:,5));
A_2=Arimax_Prediction3('PHP',0.15,5,3,'Lua.jpg');
B_2=Arimax_Prediction3('PHP',0.15,5,4,'Lua.jpg');
C_2=Arimax_Prediction3('PHP',0.15,5,5,'Lua.jpg');
Data_1=0.1*A_1+0.5*B_1+0.4*C_1;
Data_2=0.1*A_2(:,2)+0.5*B_2(:,2)+0.4*C_2(:,2);
a=figure('visible','off');
h1=plot(dates,Data_1,'-b*');
hold on;
h2=plot(A_2(:,1),Data_2,'-r*');
legend([h1 h2],{'Data','Prediction'})
title('PHP');
grid on;
saveas(a,'Proba.jpg');

