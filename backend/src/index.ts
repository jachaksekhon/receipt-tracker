import { User } from './generated/prisma/index.d';
import express from 'express';
import cors from 'cors';
import { PrismaClient } from '@prisma/client';
import userRoutes from './routes/userRoutes';

const app = express();
const prisma = new PrismaClient();

app.use(cors());
app.use(express.json());

app.get('/ping', (req, res) => {
    res.json({message: 'pong'})
})

app.use('/users', userRoutes);

// Start server
const PORT = process.env.PORT || 4000;
app.listen(PORT, () => {
  console.log(`🚀 Server listening on http://localhost:${PORT}`);
});