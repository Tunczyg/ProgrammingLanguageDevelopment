bigData = []

%wczytaj dane i po³¹cz tabice

for name = [2012:1:2019]
    iter = 1;
    fid = fopen(int2str(name)+".txt");
    tline = fgetl(fid);
    while ischar(tline)
        tabPush(iter) = jsondecode(tline);
        tline = fgetl(fid);
        iter = iter + 1;
    end
    fclose(fid);
    bigData = [bigData, tabPush];
end

%sort
[~,index] = sortrows({bigData.LanguageName}.'); 
bigData = bigData(index); 
clear index
% 
% 
%slownik
names = {bigData.LanguageName}.';
[slownik, ia, enumCol] = unique(names);

%conv2mat
cells(:,:) = struct2cell(bigData(:));
Tcells = cells.';
tempMat(:,:) = cell2mat(Tcells(:,2:9));
tempData = [enumCol, tempMat(:,1)+ tempMat(:,2)/4 - 1/4,tempMat(:,[3:8])]
    
%stworz wektory cykliczne
langCols = repelem([1:96], 30);
langCols = langCols'
year  = repmat([2012:0.25:2019.25], 1, 96)'



FinalTab = zeros([2880, 8]);
FinalTab(:,[1, 2]) = [langCols, year];



%fill zeros
for iter = [1:1:2880]
    for smallIter = [1:1:1868]
        if FinalTab(iter, [1, 2]) == tempData(smallIter,[1,2])
            FinalTab(iter, [3:1:8]) = tempData(smallIter, [3:1:8]);
        end
    end
end

