clear all;
close all;
clc;

load slownik;
% btab(:,:) = struct2cell(slownik(:));
% ctab = btab';
% s=size(ctab(:,1)); s = s(1);

invalid = [];
names = string(slownik(:,1));
size = size(slownik,1);
for i = 1:size-1
    for j = i+1:size
        if((names(i,1) == names(j,1)) && names(i,1) ~= " ")
            names(j,1) = " ";
        end
    end
end

names(names == " ") = []; %names without repeating

for i = 1:length(names)
    fprintf('STATUS FOR %s/%s\n',num2str(i),num2str(size))
    try
        Future_Prediction(i, 0.2, 8, strcat(names(i),'.jpg'),1, 1, 1, 1 ,1, 1);
    catch
        invalid = [invalid; names(i)];
        fprintf('#%s FAILED. \n',num2str(i))
        continue
    end
end

i
