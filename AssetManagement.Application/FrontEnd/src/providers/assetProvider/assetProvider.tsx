import axios from "axios";
import { stringify } from "query-string";
import { CreateParams, CreateResult, DataProvider, DeleteManyParams, DeleteManyResult, DeleteParams, DeleteResult, GetListParams, GetListResult, GetManyParams, GetManyReferenceParams, GetManyReferenceResult, GetManyResult, GetOneParams, GetOneResult, RaRecord, UpdateManyParams, UpdateManyResult, UpdateParams, UpdateResult, fetchUtils } from "ra-core";
import axiosInstance from "../../connectionConfigs/axiosInstance";
import config from "../../connectionConfigs/config.json";

export const assetProvider: DataProvider = {
    // getList: (resource, params) => {

    // },
    getOne: function <RecordType extends RaRecord = any>(resource: string, params: GetOneParams<any>): Promise<GetOneResult<RecordType>> {
        return axiosInstance.get(`/api/${resource}/${params.id}`).then(res => {
            return res
        })
    },
    // getOne: (resource, params) =>
    //     httpClient(`${apiUrl}/${resource}/${params.id}`).then(({ json }) => ({
    //         data: json,
    //     })),
    getMany: function <RecordType extends RaRecord = any>(resource: string, params: GetManyParams): Promise<GetManyResult<RecordType>> {
        throw new Error("Function not implemented.");
    },
    getManyReference: function <RecordType extends RaRecord = any>(resource: string, params: GetManyReferenceParams): Promise<GetManyReferenceResult<RecordType>> {
        throw new Error("Function not implemented.");
    },
    update: (resource, params) =>
        axiosInstance.put(`/api/${resource}/${params.id}`, params.data).then(res => res),
    updateMany: function <RecordType extends RaRecord = any>(resource: string, params: UpdateManyParams<any>): Promise<UpdateManyResult<RecordType>> {
        throw new Error("Function not implemented.");
    },
    create: function <RecordType extends RaRecord = any>(resource: string, params: CreateParams<any>): Promise<CreateResult<RecordType>> {
        return axiosInstance.post(`/api/${resource}`, params.data).then(res => { res.data.id = 9; return res });
    },
    delete: async (resource, params) => {
        const url = `${config.api.base}/api/${resource}/${params.id}`;
        var response = await axiosInstance.delete(url);
        return response.data;
    },
    deleteMany: function <RecordType extends RaRecord = any>(resource: string, params: DeleteManyParams<RecordType>): Promise<DeleteManyResult<RecordType>> {
        throw new Error("Function not implemented.");
    },
    getList: function <RecordType extends RaRecord = any>(resource: string, params: GetListParams): Promise<GetListResult<RecordType>> {
        const { page, perPage } = params.pagination;
        const { states, searchString, categories } = params.filter;
        const { field, order } = params.sort;
        let tmp = "";
        for (const key in states) {
            if (Object.prototype.hasOwnProperty.call(states, key)) {
                const element = states[key];
                tmp += element + "&";
            }
        }
        let tmp1 = "";
        for (const key in categories) {
            if (Object.prototype.hasOwnProperty.call(categories, key)) {
                const element = categories[key];
                tmp1 += element + "&";
            }
        }
        console.log(params);
        const query = {
            end: JSON.stringify((page) * perPage),
            start: JSON.stringify((page - 1) * perPage),
            sort: field,
            order: order,
            stateFilter: tmp ? tmp : null,
            searchString: searchString,
            categoryFilter: tmp1 ? tmp1 : null
        };
        const url = `/api/${resource}?${stringify(query)}`;
        return axiosInstance(url).then(res => {
            return Promise.resolve({ data: res.data.data, total: res.data.total });
        });
    }
};