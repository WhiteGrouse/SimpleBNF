# C++ Mangled Symbol Parser
## PROBLEM
- &lt;prefix&gt;のパースにおいて&lt;unqualified-name&gt;が優先されてしまい&lt;prefix&gt;&lt;unqualified-name&gt;が使用されない問題有り。だからと言って後者の先に記述すると無限ループが発生してしまう。