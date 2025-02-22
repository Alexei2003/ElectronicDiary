prompt PL/SQL Developer Export Tables for user SYS@//77.222.37.9:1521/FREEPDB1
prompt Created by alex on четверг, 20 февраль 2025 г.
set feedback off
set define off

prompt Creating ELD_ADMINISTRATORS...
create table ELD_ADMINISTRATORS
(
  id     NUMBER not null,
  a_u_id NUMBER not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table ELD_ADMINISTRATORS
  add constraint A_ID primary key (ID)
  using index
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table ELD_ADMINISTRATORS
  add constraint A_U_ID foreign key (A_U_ID)
  references ELD_USERS (ID);

prompt Creating ELD_TEACHERS...
create table ELD_TEACHERS
(
  id     NUMBER not null,
  t_u_id NUMBER not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table ELD_TEACHERS
  add constraint T_ID primary key (ID)
  using index
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table ELD_TEACHERS
  add constraint T_U_ID foreign key (T_U_ID)
  references ELD_USERS (ID);

prompt Creating ELD_CLASSES...
create table ELD_CLASSES
(
  id     NUMBER not null,
  name   NVARCHAR2(100) not null,
  c_t_id NUMBER not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table ELD_CLASSES
  add constraint C_ID primary key (ID)
  using index
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table ELD_CLASSES
  add constraint C_T_ID foreign key (C_T_ID)
  references ELD_TEACHERS (ID);

prompt Creating ELD_EDUCATIONAL_INSTITUTIONS_TYPES...
create table ELD_EDUCATIONAL_INSTITUTIONS_TYPES
(
  id   NUMBER(19) generated always as identity,
  name VARCHAR2(255 CHAR) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table ELD_EDUCATIONAL_INSTITUTIONS_TYPES
  add primary key (ID)
  using index
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

prompt Creating ELD_EDUCATIONAL_INSTITUTIONS...
create table ELD_EDUCATIONAL_INSTITUTIONS
(
  id           NUMBER(19) generated always as identity,
  address      VARCHAR2(255 CHAR) not null,
  email        VARCHAR2(255 CHAR),
  name         VARCHAR2(255 CHAR) not null,
  path_image   VARCHAR2(255 CHAR),
  phone_number VARCHAR2(255 CHAR) not null,
  ei_eit_id    NUMBER(19) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table ELD_EDUCATIONAL_INSTITUTIONS
  add primary key (ID)
  using index
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table ELD_EDUCATIONAL_INSTITUTIONS
  add constraint EI_EIT_ID foreign key (EI_EIT_ID)
  references ELD_EDUCATIONAL_INSTITUTIONS_TYPES (ID);

prompt Creating ELD_IMAGES...
create table ELD_IMAGES
(
  id          NUMBER(19) generated always as identity,
  path_images VARCHAR2(255 CHAR) not null,
  i_ei_id     NUMBER(19) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table ELD_IMAGES
  add primary key (ID)
  using index
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table ELD_IMAGES
  add constraint I_EI_ID foreign key (I_EI_ID)
  references ELD_EDUCATIONAL_INSTITUTIONS (ID);

prompt Creating ELD_PARENTS...
create table ELD_PARENTS
(
  id           NUMBER not null,
  p_u_id       NUMBER not null,
  last_name    NVARCHAR2(100) not null,
  first_name   NVARCHAR2(100) not null,
  patronymic   NVARCHAR2(100),
  path_image   NVARCHAR2(100),
  email        NVARCHAR2(100),
  phone_number NVARCHAR2(100)
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table ELD_PARENTS
  add constraint P_ID primary key (ID)
  using index
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table ELD_PARENTS
  add constraint P_U_ID foreign key (P_U_ID)
  references ELD_USERS (ID);

prompt Creating ELD_SCHOOL_STUDENTS...
create table ELD_SCHOOL_STUDENTS
(
  id              NUMBER not null,
  sst_u_id        NUMBER not null,
  sst_c_id        NUMBER not null,
  sst_p_father_id NUMBER,
  sst_p_mother_id NUMBER
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table ELD_SCHOOL_STUDENTS
  add constraint SST_ID primary key (ID)
  using index
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table ELD_SCHOOL_STUDENTS
  add constraint SST_C_ID foreign key (SST_C_ID)
  references ELD_CLASSES (ID);
alter table ELD_SCHOOL_STUDENTS
  add constraint SST_P_FATHER_ID foreign key (SST_P_FATHER_ID)
  references ELD_PARENTS (ID);
alter table ELD_SCHOOL_STUDENTS
  add constraint SST_P_MOTHER_ID foreign key (SST_P_MOTHER_ID)
  references ELD_PARENTS (ID);
alter table ELD_SCHOOL_STUDENTS
  add constraint SST_U_ID foreign key (SST_U_ID)
  references ELD_USERS (ID);

prompt Creating ELD_SCHOOL_SUBJECTS...
create table ELD_SCHOOL_SUBJECTS
(
  id   NUMBER not null,
  name NVARCHAR2(100) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table ELD_SCHOOL_SUBJECTS
  add constraint SS_ID primary key (ID)
  using index
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

prompt Creating ELD_TEACHER_ASSIGNMENTS...
create table ELD_TEACHER_ASSIGNMENTS
(
  id       NUMBER not null,
  ta_t_id  NUMBER not null,
  ta_ss_id NUMBER not null,
  ta_c_id  NUMBER not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table ELD_TEACHER_ASSIGNMENTS
  add constraint TA_ID primary key (ID)
  using index
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table ELD_TEACHER_ASSIGNMENTS
  add constraint TA_C_ID foreign key (TA_C_ID)
  references ELD_CLASSES (ID);
alter table ELD_TEACHER_ASSIGNMENTS
  add constraint TA_SS_ID foreign key (TA_SS_ID)
  references ELD_SCHOOL_SUBJECTS (ID);
alter table ELD_TEACHER_ASSIGNMENTS
  add constraint TA_T_ID foreign key (TA_T_ID)
  references ELD_TEACHERS (ID);

prompt Creating ELD_USERS_TYPES...
create table ELD_USERS_TYPES
(
  id   NUMBER(19) generated always as identity,
  name VARCHAR2(255 CHAR) not null
)
tablespace SYSTEM
  pctfree 10
  pctused 40
  initrans 1
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );
alter table ELD_USERS_TYPES
  add primary key (ID)
  using index
  tablespace SYSTEM
  pctfree 10
  initrans 2
  maxtrans 255
  storage
  (
    initial 64K
    next 1M
    minextents 1
    maxextents unlimited
  );

prompt Disabling triggers for ELD_ADMINISTRATORS...
alter table ELD_ADMINISTRATORS disable all triggers;
prompt Disabling triggers for ELD_TEACHERS...
alter table ELD_TEACHERS disable all triggers;
prompt Disabling triggers for ELD_CLASSES...
alter table ELD_CLASSES disable all triggers;
prompt Disabling triggers for ELD_EDUCATIONAL_INSTITUTIONS_TYPES...
alter table ELD_EDUCATIONAL_INSTITUTIONS_TYPES disable all triggers;
prompt Disabling triggers for ELD_EDUCATIONAL_INSTITUTIONS...
alter table ELD_EDUCATIONAL_INSTITUTIONS disable all triggers;
prompt Disabling triggers for ELD_IMAGES...
alter table ELD_IMAGES disable all triggers;
prompt Disabling triggers for ELD_PARENTS...
alter table ELD_PARENTS disable all triggers;
prompt Disabling triggers for ELD_SCHOOL_STUDENTS...
alter table ELD_SCHOOL_STUDENTS disable all triggers;
prompt Disabling triggers for ELD_SCHOOL_SUBJECTS...
alter table ELD_SCHOOL_SUBJECTS disable all triggers;
prompt Disabling triggers for ELD_TEACHER_ASSIGNMENTS...
alter table ELD_TEACHER_ASSIGNMENTS disable all triggers;
prompt Disabling triggers for ELD_USERS_TYPES...
alter table ELD_USERS_TYPES disable all triggers;
prompt Disabling foreign key constraints for ELD_ADMINISTRATORS...
alter table ELD_ADMINISTRATORS disable constraint A_U_ID;
prompt Disabling foreign key constraints for ELD_TEACHERS...
alter table ELD_TEACHERS disable constraint T_U_ID;
prompt Disabling foreign key constraints for ELD_CLASSES...
alter table ELD_CLASSES disable constraint C_T_ID;
prompt Disabling foreign key constraints for ELD_EDUCATIONAL_INSTITUTIONS...
alter table ELD_EDUCATIONAL_INSTITUTIONS disable constraint EI_EIT_ID;
prompt Disabling foreign key constraints for ELD_IMAGES...
alter table ELD_IMAGES disable constraint I_EI_ID;
prompt Disabling foreign key constraints for ELD_PARENTS...
alter table ELD_PARENTS disable constraint P_U_ID;
prompt Disabling foreign key constraints for ELD_SCHOOL_STUDENTS...
alter table ELD_SCHOOL_STUDENTS disable constraint SST_C_ID;
alter table ELD_SCHOOL_STUDENTS disable constraint SST_P_FATHER_ID;
alter table ELD_SCHOOL_STUDENTS disable constraint SST_P_MOTHER_ID;
alter table ELD_SCHOOL_STUDENTS disable constraint SST_U_ID;
prompt Disabling foreign key constraints for ELD_TEACHER_ASSIGNMENTS...
alter table ELD_TEACHER_ASSIGNMENTS disable constraint TA_C_ID;
alter table ELD_TEACHER_ASSIGNMENTS disable constraint TA_SS_ID;
alter table ELD_TEACHER_ASSIGNMENTS disable constraint TA_T_ID;
prompt Loading ELD_ADMINISTRATORS...
prompt Table is empty
prompt Loading ELD_TEACHERS...
prompt Table is empty
prompt Loading ELD_CLASSES...
prompt Table is empty
prompt Loading ELD_EDUCATIONAL_INSTITUTIONS_TYPES...
prompt Table is empty
prompt Loading ELD_EDUCATIONAL_INSTITUTIONS...
prompt Table is empty
prompt Loading ELD_IMAGES...
prompt Table is empty
prompt Loading ELD_PARENTS...
prompt Table is empty
prompt Loading ELD_SCHOOL_STUDENTS...
prompt Table is empty
prompt Loading ELD_SCHOOL_SUBJECTS...
prompt Table is empty
prompt Loading ELD_TEACHER_ASSIGNMENTS...
prompt Table is empty
prompt Loading ELD_USERS_TYPES...
insert into ELD_USERS_TYPES (id, name)
values (1, 'Main admin');
insert into ELD_USERS_TYPES (id, name)
values (6, 'Local admin');
commit;
prompt 2 records loaded
prompt Enabling foreign key constraints for ELD_ADMINISTRATORS...
alter table ELD_ADMINISTRATORS enable constraint A_U_ID;
prompt Enabling foreign key constraints for ELD_TEACHERS...
alter table ELD_TEACHERS enable constraint T_U_ID;
prompt Enabling foreign key constraints for ELD_CLASSES...
alter table ELD_CLASSES enable constraint C_T_ID;
prompt Enabling foreign key constraints for ELD_EDUCATIONAL_INSTITUTIONS...
alter table ELD_EDUCATIONAL_INSTITUTIONS enable constraint EI_EIT_ID;
prompt Enabling foreign key constraints for ELD_IMAGES...
alter table ELD_IMAGES enable constraint I_EI_ID;
prompt Enabling foreign key constraints for ELD_PARENTS...
alter table ELD_PARENTS enable constraint P_U_ID;
prompt Enabling foreign key constraints for ELD_SCHOOL_STUDENTS...
alter table ELD_SCHOOL_STUDENTS enable constraint SST_C_ID;
alter table ELD_SCHOOL_STUDENTS enable constraint SST_P_FATHER_ID;
alter table ELD_SCHOOL_STUDENTS enable constraint SST_P_MOTHER_ID;
alter table ELD_SCHOOL_STUDENTS enable constraint SST_U_ID;
prompt Enabling foreign key constraints for ELD_TEACHER_ASSIGNMENTS...
alter table ELD_TEACHER_ASSIGNMENTS enable constraint TA_C_ID;
alter table ELD_TEACHER_ASSIGNMENTS enable constraint TA_SS_ID;
alter table ELD_TEACHER_ASSIGNMENTS enable constraint TA_T_ID;
prompt Enabling triggers for ELD_ADMINISTRATORS...
alter table ELD_ADMINISTRATORS enable all triggers;
prompt Enabling triggers for ELD_TEACHERS...
alter table ELD_TEACHERS enable all triggers;
prompt Enabling triggers for ELD_CLASSES...
alter table ELD_CLASSES enable all triggers;
prompt Enabling triggers for ELD_EDUCATIONAL_INSTITUTIONS_TYPES...
alter table ELD_EDUCATIONAL_INSTITUTIONS_TYPES enable all triggers;
prompt Enabling triggers for ELD_EDUCATIONAL_INSTITUTIONS...
alter table ELD_EDUCATIONAL_INSTITUTIONS enable all triggers;
prompt Enabling triggers for ELD_IMAGES...
alter table ELD_IMAGES enable all triggers;
prompt Enabling triggers for ELD_PARENTS...
alter table ELD_PARENTS enable all triggers;
prompt Enabling triggers for ELD_SCHOOL_STUDENTS...
alter table ELD_SCHOOL_STUDENTS enable all triggers;
prompt Enabling triggers for ELD_SCHOOL_SUBJECTS...
alter table ELD_SCHOOL_SUBJECTS enable all triggers;
prompt Enabling triggers for ELD_TEACHER_ASSIGNMENTS...
alter table ELD_TEACHER_ASSIGNMENTS enable all triggers;
prompt Enabling triggers for ELD_USERS_TYPES...
alter table ELD_USERS_TYPES enable all triggers;

set feedback on
set define on
prompt Done
