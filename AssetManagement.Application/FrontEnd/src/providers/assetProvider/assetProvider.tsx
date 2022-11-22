import axios from "axios";
import { stringify } from "query-string";
import { CreateParams, CreateResult, DataProvider, DeleteManyParams, DeleteManyResult, DeleteParams, DeleteResult, GetManyParams, GetManyReferenceParams, GetManyReferenceResult, GetManyResult, GetOneParams, GetOneResult, RaRecord, UpdateManyParams, UpdateManyResult, UpdateParams, UpdateResult } from "ra-core";
import config from "../../connectionConfigs/config.json";

export const assetProvider: DataProvider = {
   getList: (resource, params) => {
      const { page, perPage } = params.pagination;
      const query = {
         pageIndex: JSON.stringify(page),
         pageSize: JSON.stringify(perPage),
      };
      const url = `${config.api.base}/${resource}?${stringify(query)}`;
      console.log(perPage);
      return axios(url).then(res => {
         return res;
      });
   },
   getOne: function <RecordType extends RaRecord = any>(resource: string, params: GetOneParams<any>): Promise<GetOneResult<RecordType>> {
      throw new Error("Function not implemented.");
   },
   getMany: function <RecordType extends RaRecord = any>(resource: string, params: GetManyParams): Promise<GetManyResult<RecordType>> {
      throw new Error("Function not implemented.");
   },
   getManyReference: function <RecordType extends RaRecord = any>(resource: string, params: GetManyReferenceParams): Promise<GetManyReferenceResult<RecordType>> {
      throw new Error("Function not implemented.");
   },
   update: function <RecordType extends RaRecord = any>(resource: string, params: UpdateParams<any>): Promise<UpdateResult<RecordType>> {
      throw new Error("Function not implemented.");
   },
   updateMany: function <RecordType extends RaRecord = any>(resource: string, params: UpdateManyParams<any>): Promise<UpdateManyResult<RecordType>> {
      throw new Error("Function not implemented.");
   },
   create: function <RecordType extends RaRecord = any>(resource: string, params: CreateParams<any>): Promise<CreateResult<RecordType>> {
      throw new Error("Function not implemented.");
   },
   delete: function <RecordType extends RaRecord = any>(resource: string, params: DeleteParams<RecordType>): Promise<DeleteResult<RecordType>> {
      throw new Error("Function not implemented.");
   },
   deleteMany: function <RecordType extends RaRecord = any>(resource: string, params: DeleteManyParams<RecordType>): Promise<DeleteManyResult<RecordType>> {
      throw new Error("Function not implemented.");
   },
};