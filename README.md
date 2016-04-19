Compare two CSV files and output results.

Originally written to help compare contrast  vulnerabilities.
Core functionality is in ContrastCompare.dll
May eventually expand to use the Contrast rest API.

Usage:
CCompare.exe oldFile.csv newFile.csv outFile.txt
Compares oldFile.csv against newFile.csv looking for added rows, removed rows, and unchanged rows.
Edit config.ini for more options. Config descriptions below:


# For setting up contrast API key (not implemented yet)
apikey=not used yet

# What columns to include in the comparison (comma separated)
include_columns=column1,column2

# What columns should be named when output to the report. Keep in same order as include_columns
output_columns=out_column1,out_column2

# Ignores case when comparing cells in a CSV row (does not affect column names, these are always case sensitive)
case_sensitive=false

# Prune out rows that have a specific string in a certain cell. Ex. if column2 is set to the string "exclude" it will be counted as removed. Comma separate for multiple cases
prune_rows=column2|exclude,column2|remove