INSERT INTO ocaslr_voucherBase
  ( ocaslr_voucherId
  , CreatedOn
  , CreatedBy
  , ModifiedOn
  , ModifiedBy
  , OwnerId
  , OwnerIdType
  , OwningBusinessUnit
  , statecode
  , statuscode
  , ocaslr_name
  , ocaslr_State
  , ocaslr_Value
  , TransactionCurrencyId
  , ExchangeRate
  , ocaslr_DateIssued
  , ocaslr_BatchNumber
  , ocaslr_Partner
  , ocaslr_Product
  , ocaslr_value_Base)
VALUES
  ( NEWID() --ocaslr_voucherId
  , GETUTCDATE() --CreatedOn
  , '77FA9B9B-88E6-E311-80C1-00155D157F22' --CreatedBy
  , GETUTCDATE() --ModifiedOn
  , '77FA9B9B-88E6-E311-80C1-00155D157F22' --ModifiedBy
  , (SELECT TOP 1 OwnerID FROM OwnerBase) --OwnerId
  , (SELECT TOP 1 OwnerIdType FROM OwnerBase) --OwnerIdType
  , (SELECT TOP 1 BusinessUnitId FROM BusinessUnitBase) --OwningBusinessUnit
  , 0 --statecode
  , 1 --statuscode
  , @VoucherCode --ocaslr_name
  , 1 --ocaslr_State
  , 95 --ocaslr_Value
  , (SELECT TOP 1 TransactionCurrencyId FROM TransactionCurrencyBase) --TransactionCurrencyId
  , 1 --ExchangeRate
  , GETUTCDATE() --ocaslr_DateIssued
  , @VoucherCode --ocaslr_BatchNumber
  , @CollegeId --ocaslr_Partner
  , (SELECT TOP 1 ProductId FROM [ProductBase] WHERE [ocaslr_applicationcycleid] = @ApplicationCycleId AND ocaslr_effectivedate < GETUTCDATE() ORDER BY ocaslr_effectivedate DESC) --ocaslr_Product
  , 95) --ocaslr_value_Base