name = 'pchip'
for iter = [1:30:2880]
    BigTabNan([iter:1:iter+29], [3:1:8]) = [fillmissing(BigTabNan([iter:1:iter+29], [3]),name),fillmissing(BigTabNan([iter:1:iter+29], [4]),name),fillmissing(BigTabNan([iter:1:iter+29], [5]),name),fillmissing(BigTabNan([iter:1:iter+29], [6]),name),fillmissing(BigTabNan([iter:1:iter+29], [7]),name),fillmissing(BigTabNan([iter:1:iter+29], [8]),name)];
end