update Doctor set LastWorkDay='01/01/2000', WorkingDays=0, WeeklyHours=0 where DoctorID>=1;
truncate table DoctorShifts;