INSERT INTO "tb_User"
  (
    "Id",
    "IsDel",
    "Name",
    "Sex",
    "Account",
    "Passwd",
    "SignupDate",
    "RoleId",
    "Status",
    "Avatar"
  )
  VALUES
  (
    '5af390dc3b3128f', -- "Id"
    0, -- "IsDel"
    '超级管理员', -- "Name"
    1, -- "Sex"
    'admin', -- "Account"
    '123456', -- "Passwd"
    14858714893431195, -- "SignupDate"
    '0000000000000000', -- "RoleId"
    0, -- "Status"
    '0000000000000000' -- "Avatar"
  );

  INSERT INTO "tb_Config"
  (
    "Id",
    "IsDel",
    "Name",
    "Type",
    "Data"
  )
  VALUES
  (
    '5af390dc3b3128f', -- "Id"
    0, -- "IsDel"
    '首页表格提示文本', -- "Name"
    1, -- "Type"
    '首页表格提示文本' -- "Data"
  );