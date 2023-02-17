select * from MemberMaster where isnull(Facility,'')=''

--update MemberMaster set Facility='MLTC' where Facility='Centers'
--update MemberMaster set Facility='MAP' where Facility='Centers MAP'
--update MemberMaster set Facility='DSNP' where Facility='Centers DSNP'
--update MemberMaster set Facility='MLTC' where Facility='Centers Surplus'

select Distinct(Facility) from MemberMaster 