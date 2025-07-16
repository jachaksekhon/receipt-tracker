import { PrismaClient } from '@prisma/client';

const prisma = new PrismaClient();

export const getUsers = async () => {
  return await prisma.user.findMany();
};

export const createUser = async (data: any) => {
    return await prisma.user.create({ data });
}

export const findUserByEmail = async (email: String) => {
    return await prisma.user.findUnique({ where: { email }})
}