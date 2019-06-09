clear all;
close all;
clc;

load tab;
btab(:,:) = struct2cell(tab(:));
ctab = btab';
s=size(ctab(:,1)); s = s(1);

names = string(ctab(:,1));
for i = 1:s-1
    for j = i+1:s
        if((names(i,1) == names(j,1)) && names(i,1) ~= " ")
            names(j,1) = " ";
        end
    end
end

names(names == " ") = []; %names without repeating

for i = 1:length(names)
        Future_Prediction(names(i), 0.15, 5, names(i)+'.jpg',0.33, 0.33, 0.33);
end
